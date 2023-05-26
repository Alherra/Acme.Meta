using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// ServiceBase
    /// </summary>
    [AutoInject]
    [Description("ServiceBase")]
    public abstract class AppService
    {
        /// <summary>
        /// CurrentUser
        /// </summary>
        [Description("CurrentUser")]
        protected UserInfo? CurrentUser 
            => JsonConvert.DeserializeObject<UserInfo>(ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CurrentUser")?.Value! ?? string.Empty);

        /// <summary>
        /// Creates an instance of the specified type using that type's parameterless constructor.
        /// </summary>
        /// <typeparam name="TClass">type</typeparam>
        /// <returns></returns>
        [Description("Creates an instance of the specified type using that type's parameterless constructor")]
        protected static TClass CreateInstance<TClass>(string func = "", params Object[] objects) => 
            func.Equals("") 
            ? Activator.CreateInstance<TClass>()
            : (TClass)(typeof(TClass).GetMethod(func)?.Invoke(null!, objects) ?? default(TClass))!;
    }
}
