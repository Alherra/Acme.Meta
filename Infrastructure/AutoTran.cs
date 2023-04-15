using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Filters
{
    /// <summary>
    /// 数据事务自动过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [Description("数据事务自动过滤")]
    public class AutoTran : ActionFilterAttribute
    {
        /// <summary>
        /// 是否过滤
        /// </summary>
        [Description("是否过滤")]
        public bool IsTran { get; set; } = false;

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        [Description("执行前")]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 非查询类型接口
            if (IsTran)
            {
                // 事务引擎
                DbScoped.SugarScope.BeginTran();
            }
            base.OnActionExecuting(context);
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
            // 非查询类型接口
            if (IsTran)
            {
                // 事务引擎
                if (context.HttpContext.Response.StatusCode < 205)
                    DbScoped.SugarScope.CommitTran();
                else
                    DbScoped.SugarScope.RollbackTran();
            }
        }
    }
}
