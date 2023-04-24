using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Org.BouncyCastle.Bcpg.Sig;

namespace System
{
    /// <summary>
    /// ServiceProvider
    /// </summary>
    internal class ServiceProvider
    {
        /// <summary>
        /// Http Context
        /// </summary>
        public static HttpContext HttpContext => Activator.CreateInstance<HttpContextAccessor>().HttpContext;

        /// <summary>
        /// IServiceProvider
        /// </summary>
        public static IServiceProvider Instance => HttpContext.Features.Get<IServiceProvidersFeature>()?.RequestServices!;

        /// <summary>
        /// Get instance for service.
        /// </summary>
        /// <typeparam name="TService">The type of service.</typeparam>
        /// <returns>The instance for service.</returns>
        public static TService? GetService<TService>() => (TService)Instance?.GetService(typeof(TService))!;
    }
}
