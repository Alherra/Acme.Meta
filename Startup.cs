using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.ComponentModel;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Meta初始化配置
    /// </summary>
    [Description("Meta初始化配置")]
    public static class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        [Description("Start Meta")]
        public static IServiceCollection RunMeta(this IServiceCollection services)
        {
            // DependencyInject
            services.AutoInjection();

            // Config redis
            if (!string.IsNullOrEmpty(AppSetting.Get("Redis.Configuration")))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = AppSetting.Get("Redis.Configuration");
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    {
                        DefaultDatabase = int.Parse(AppSetting.Get("Redis.DefaultDataBase")),
                        Password = AppSetting.Get("Redis.Password") ?? String.Empty
                    };
                });
            }

            Task.Run(InitSugarTables);
            return services;
        }

        /// <summary>
        /// Dependency
        /// 
        /// Auto Injection.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        [Description("DependencyInject")]
        private static IServiceCollection AutoInjection(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<AppLogger>();
            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.TryAddScoped<IAccountService, AccountService>();
            serviceCollection.TryAddSingleton<IEncrypter, Encrypter>();
            serviceCollection.TryAddSingleton<IRedis>(RedisClient.Instance);
            serviceCollection.TryAddSingleton<IEmail, Email>();
            serviceCollection.TryAddSingleton<ITencentSMS, TencentSMS>();

            #region Config Dependencies
            var services = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                .Select(Assembly.LoadFile)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<AutoInjectAttribute>() != null)
                .ToList();
            if (services.Count <= 0)
                return serviceCollection;

            foreach (var service in services)
            {
                var attr = service.GetCustomAttribute<AutoInjectAttribute>();
                foreach (var implement in service.GetInterfaces().Where(i => i.Name.Equals('I' + service.Name)))
                    switch (attr?.InjectType)
                    {
                        case InjectType.Scope:
                            serviceCollection.TryAddScoped(implement, service);
                            break;
                        case InjectType.Single:
                            serviceCollection.TryAddSingleton(implement, service);
                            break;
                        case InjectType.Transient:
                            serviceCollection.TryAddTransient(implement, service);
                            break;
                        default:
                            continue;
                    }
            }
            #endregion

            return serviceCollection;
        }

        /// <summary>
        /// Initial Database
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        [Description("Refresh tables")]
        private static async void InitSugarTables()
        {
            try
            {
                Thread.Sleep(1000);
                await AppDB.Execute(async db =>
                {
                    var tables = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                        .Select(Assembly.LoadFrom)
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.CustomAttributes != null && t.CustomAttributes.Any(a => a.AttributeType.Name == "SugarTable"))
                        .ToArray();
                    ((SqlSugar.SqlSugarClient)db).CodeFirst.InitTables(tables);

                    return await Task.FromResult(true);
                });
            }
            catch
            {
                var task = Task.Run(InitSugarTables);
            }
        }

    }
}
