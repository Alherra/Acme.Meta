using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// Extensions for http errors.
    /// </summary>
    [Description("Extensions for http errors")]
    public static class HttpErrorExtension
    {
        /// <summary>
        /// ActionExecutingContextError
        /// </summary>
        [Description("ActionExecutingContextError")]
        public static void MapError(this ActionExecutingContext context, StatusCode status, string message, string details, string code = "")
        {
            context.Result = new JsonResult(new
            {
                Error = new
                {
                    Code = string.IsNullOrEmpty(code) ? status.ToString() : code,
                    Message = string.IsNullOrEmpty(message) ? status.ToString() : message,
                    Details = string.IsNullOrEmpty(details) ? status.ToString() : details
                }
            });
            context.HttpContext.Response.StatusCode = (int)status;
        }

        /// <summary>
        /// ActionExecutedContextError
        /// </summary>
        [Description("ActionExecutedContextError")]
        public static void MapError(this ActionExecutedContext context, StatusCode status, string message = "", string details = "", string code = "")
        {
            context.Result = new JsonResult(new
            {
                Error = new
                {
                    Code = string.IsNullOrEmpty(code) ? status.ToString() : code,
                    Message = string.IsNullOrEmpty(message) ?  message : context.Exception.Message,
                    Details = string.IsNullOrEmpty(details) ? details : context.Exception.ToString()
                }
            });
            context.HttpContext.Response.StatusCode = (int)status;
        }

        /// <summary>
        /// AuthorizationFilterContextError
        /// </summary>
        [Description("AuthorizationFilterContextError")]
        public static void MapError(this AuthorizationFilterContext context, StatusCode status, string message, string details, string code = "")
        {
            context.Result = new JsonResult(new
            {
                Error = new
                {
                    Code = string.IsNullOrEmpty(code) ? status.ToString() : code,
                    Message = string.IsNullOrEmpty(message) ? status.ToString() : message,
                    Details = string.IsNullOrEmpty(details) ? status.ToString() : details
                }
            });
            context.HttpContext.Response.StatusCode = (int)status;
        }
    }
}
