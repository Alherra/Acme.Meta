using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using SqlSugar.IOC;
using System.Buffers;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 请求过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Description("请求过滤")]
    public class AcitonMeta : ActionFilterAttribute
    {
        /// <summary>
        /// 校验查询类请求
        /// </summary>
        [Description("校验查询类请求")]
        static readonly Func<string, bool> IsQuery =
            str => string.IsNullOrEmpty(str)
            || str.ToUpper().Contains("GET") || str.ToUpper().Contains("QUERY") || str.ToUpper().Contains("DETAIL")
            || str.Equals("KeepAlive");

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        [Description("执行前")]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var connectionId = context.HttpContext.Connection.Id;
            // 过滤高频请求（5秒内20次）
            if (CacheServer.CheckRequestLocked(connectionId) || !CacheServer.RecordRequestLimited(connectionId))
            {
                context.MapError(StatusCode.OverFrequency, "Request Too Much", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                return;
            }
            try
            {
                // 进入action之前
                var controller = context.RouteData.Values["controller"]?.ToString();
                var request = context.HttpContext.Request;
                var ip = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                // 记录请求日志
                MetaLogger.Request($"Request: {context.HttpContext.TraceIdentifier}\n{request.Method} {request.Path}{request.QueryString}\n");
                var db = DbScoped.SugarScope;
                var key = context.HttpContext.Connection.Id;

                //base.OnActionExecuting(context);
                try
                {
                    if (!IsQuery(context.RouteData.Values["action"]?.ToString()!))
                        db.BeginTran();
                }
                catch (Exception)
                {
                    context.MapError(StatusCode.NotFound, "Error Request", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                }
            }
            catch (Exception ex)
            {
                context.MapError(StatusCode.Server, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// 执行结果处理
        /// 
        /// 进入action之后，返回result前
        /// </summary>
        /// <param name="context"></param>
        [Description("执行结果处理")]
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // 事务引擎
            if (context.Exception is null)
            {
                // 非查询类型接口
                if (!IsQuery(context.RouteData.Values["action"]?.ToString()!))
                   DbScoped.SugarScope.CommitTran();
            }
            // 编辑异常信息
            else
            {
                // 处理异常
                switch (context.Exception.GetType().Name)
                {
                    // 业务逻辑异常
                    case "BussinessException":
                        context.MapError(StatusCode.BussinessError);
                        break;
                    // 身份验证异常
                    case "AuthorizeException":
                        context.MapError(StatusCode.UnAuthorize);
                        break;
                    // 无效参数异常
                    case "InvalidArgumentException":
                        context.MapError(StatusCode.DataMapperError);
                        break;
                    // 其他异常
                    default:
                        if (context.HttpContext.Response.StatusCode >= 500)
                            context.MapError(StatusCode.Server);
                        else
                            context.MapError(StatusCode.Server, context.Exception.Source!);
                        break;
                }
                // 异常日志
                MetaLogger.Request(context.Exception.ToString());
                context.Exception = null;
            }
            // 添加请求头
            if (context.HttpContext.Response != null && !context.HttpContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "set-cookie");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", context.HttpContext.Request.Headers["origin"]);
            }
            /* Write Log */
            #region Write Log
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            MetaLogger.Request($"{request.HttpContext.Connection.RemoteIpAddress} {context.HttpContext.TraceIdentifier}\n{response?.StatusCode} {request.Method} {request.Path}{request.QueryString}\n");
            #endregion
        }

        /// <summary>
        /// 返回result之前
        /// </summary>
        /// <param name="context"></param>
        [Description("执行结果处理前")]
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            GC.Collect();
        }

        /// <summary>
        /// 返回result之后
        /// </summary>
        /// <param name="context"></param>
        [Description("执行结果处理后")]
        public override void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
