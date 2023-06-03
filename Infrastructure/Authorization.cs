using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using TencentCloud.Ccc.V20200210.Models;

namespace System
{
    /// <summary>
    /// Authorized option filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [Description("Option Authorized")]
    public class Authorization : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the policy name that determines access to the resource.
        /// </summary>
        [Description("Option policy name")]
        public string Policy { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        [Description("Option role")]
        public string Roles { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a comma delimited list of schemes from which user information is constructed.
        /// </summary>
        public string AuthenticationSchemes { get; set; } = String.Empty;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var allowany = context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute);
            var authorization = context.HttpContext.Request.Headers["Authorization"];
            var user = string.IsNullOrEmpty(authorization) ? null : ServiceProvider.GetService<IAuthorizer>()?.GetUser(authorization!);

            if (!allowany)
            {
                if (user is null || user.Id == 0)
                {
                    context.MapError(StatusCode.UnAuthorize, "State Un-Login", "Loginned Required");
                    return;
                }
                if (!string.IsNullOrEmpty(Policy) && !ServiceProvider.GetService<ISugarDB>()!.Scope.Queryable<UserOption>().Any(x => x.UserId == user.Id && x.Policy == Policy))
                {
                    context.MapError(StatusCode.Forbidden, "None Permission", "User Authorize Limited");
                    return;
                }
            }

            if (user != null)
            {
                var claims = new List<Claim> { new Claim("CurrentUser", JsonConvert.SerializeObject(user.MapTo<UserInfo>())) };
                context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
            }
        }
    }
}
