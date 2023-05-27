using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// Action Filter
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Description("Action Filter")]
    public class AcitonMeta : ActionFilterAttribute
    {
        /// <summary>
        /// OnRequest
        /// </summary>
        [Description("OnRequest")]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var controller = context.RouteData.Values["controller"]?.ToString();
                var request = context.HttpContext.Request;
                var ip = $"{context.HttpContext.Connection.RemoteIpAddress} {context.HttpContext.Request.Headers["x-real-ip"]}";

                // Record
                ServiceProvider.GetService<AppLogger>()?.Request($"Host:{request.HttpContext.Request.Headers["host"]}\nRemote:{request.HttpContext.Request.Headers["remote-host"]}\n{ip}\nRequest: {context.HttpContext.TraceIdentifier}\n{request.Method} {request.Path}{request.QueryString}\n");
            }
            catch (Exception ex)
            {
                context.MapError(StatusCode.Server, ex.Message, ex.ToString());
            }
        }

        /// <summary>
        /// ResolveResponse
        /// </summary>
        [Description("ResolveResponse")]
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var logger = ServiceProvider.GetService<AppLogger>();
            if (context.Exception != null)
            {
                switch (context.Exception.GetType().Name)
                {
                    case "BussinessException":
                        context.MapError(StatusCode.BussinessError);
                        break;
                    case "AuthorizeException":
                        context.MapError(StatusCode.UnAuthorize);
                        break;
                    case "InvalidArgumentException":
                        context.MapError(StatusCode.DataMapperError);
                        break;
                    case "FrequencyException":
                        context.MapError(StatusCode.OverFrequency, "Request Too Much", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                        break;
                    default:
                        context.MapError(StatusCode.Server, context.Exception.Source ?? string.Empty);
                        break;
                }

                logger?.Request(context.Exception.ToString());
                context.Exception = null;
            }
            // Add origin
            if (context.HttpContext.Response != null && !context.HttpContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "set-cookie");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", context.HttpContext.Request.Headers["origin"]);
            }

            // Record
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            logger?.Request($"{request.HttpContext.Connection.RemoteIpAddress} {request.HttpContext.Request.Headers["x-real-ip"]}\n{context.HttpContext.TraceIdentifier}\n{response?.StatusCode} {request.Method} {request.Path}{request.QueryString}\n");
        }

        /// <summary>
        /// GC
        /// </summary>
        [Description("GC")]
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            GC.Collect();
        }

        /// <summary>
        /// Resulted
        /// </summary>
        [Description("Resulted")]
        public override void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
