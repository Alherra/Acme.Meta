using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Mna.V20210119.Models;

namespace System
{
    /// <summary>
    /// Extensions for http errors.
    /// 
    /// 异常处理扩展
    /// </summary>
    [Description("异常处理扩展")]
    public static class ErrorExtension
    {
        /// <summary>
        /// Reset the error outputs.
        /// 
        /// 配置异常信息
        /// </summary>
        [Description("配置异常信息")]
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
        /// Reset the error outputs.
        /// 
        /// 配置异常信息
        /// </summary>
        [Description("配置异常信息")]
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
        /// Reset the error outputs.
        /// 
        /// 配置异常信息
        /// </summary>
        [Description("配置异常信息")]
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
