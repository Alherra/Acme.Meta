using Meta.Core.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;

namespace Meta
{
    /// <summary>
    /// 账户权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    [Description("账户权限")]
    public class Authorize : ActionFilterAttribute//, IAuthorizeData
    {
        /// <summary>
        /// ABP关联权限
        /// </summary>
        [Description("ABP关联权限")]
        public bool Abp { get; set; }

        /// <summary>
        /// Gets or sets the policy name that determines access to the resource.
        /// </summary>
        [Description("权限归属")]
        public string Policy { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        [Description("角色")]
        public string Roles { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a comma delimited list of schemes from which user information is constructed.
        /// </summary>
        public string AuthenticationSchemes { get; set; } = String.Empty;

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        [Description("执行前")]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = CacheServer.Find(context.HttpContext.Connection.Id);
            if (user.Id == 0)
            {
                context.MapError(StatusCode.UnAuthorize, "State Un-Login", "Loginned Required");
                return;
            }
            var action = $"{context.RouteData.Values["controller"]}.{context.RouteData.Values["action"]}";
            if (!DbService.QueryScope.Queryable<UserOption>().Any(x => x.UserId == user.Id && x.Action == action))
            {
                context.MapError(StatusCode.Forbidden, "None Permission", "User Authorize Limited");
                return;
            }
        }
    }
}
