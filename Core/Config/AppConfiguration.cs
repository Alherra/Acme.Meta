using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Meta
{
    /// <summary>
    /// 应用配置
    /// </summary>
    [Description("应用配置")]
    public class AppConfiguration
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        [Description("获取缓存")]
        private static readonly Dictionary<string, string> Cache = new();

        /// <summary>
        /// 获取配置
        /// </summary>
        [Description("获取配置")]
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 获取配置文件节点值
        /// </summary>
        [Description("获取配置文件节点值")]
        public static string GetValue(string section)
        {            
            // 校验参数合法性
            if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(section.Trim()))
                return string.Empty;

            section = section.Trim().Replace('.', ':');
            // 查询缓存
            if (Cache.ContainsKey(section))
                return Cache[section];
            var value = Configuration[section];
            Cache[section] = value!;
            return Configuration[section]!;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        static AppConfiguration()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //AppDomain.CurrentDomain.BaseDirectory是程序集基目录，所以appsettings.json,需要复制一份放在程序集目录下，
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
    }
}
