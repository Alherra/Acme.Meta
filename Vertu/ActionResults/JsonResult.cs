using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Meta
{
    /// <summary>
    /// Json
    /// </summary>
    public class JsonResult : ActionResult, IStatusCodeActionResult, IActionResult
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="value"></param>
        public JsonResult(object? value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the Microsoft.Net.Http.Headers.MediaTypeHeaderValue representing
        /// the Content-Type header of the response.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the serializer settings.
        /// When using System.Text.Json, this should be an instance of System.Text.Json.JsonSerializerOptions
        /// When using Newtonsoft.Json, this should be an instance of JsonSerializerSettings.
        /// </summary>
        public object? SerializerSettings { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the value to be formatted.
        /// </summary>
        public object? Value { get; set; }

        int? IStatusCodeActionResult.StatusCode => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
                //response.setContentType("text/html;charset=utf-8");
            }

            if (this.Value != null)
            {
                string jsonString = JsonConvert.SerializeObject(Value);
                JsonSerializerSettings set = new();
                set.NullValueHandling = NullValueHandling.Ignore;

                //string jsonString = ExtJsonManager.SerializeObject(Data);
                return response.WriteAsync(jsonString);
            }
            else
                return response.WriteAsync("{}");
        }
    }
}