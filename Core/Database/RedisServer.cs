using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Cfw.V20190904.Models;

namespace Meta
{
    /// <summary>
    /// 获取Redis服务
    /// </summary>
    [Description("获取Redis服务")]
    public class RedisServer
    {
        private readonly static IDatabase _redis = RedisClient.GetDatabase();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Description("查询")]
        public static T? Get<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(_redis.StringGet(key));
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Description("设置")]
        public static bool Set<T>(object key, T value)
        {
            return _redis.StringSet(key.ToString(), JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Description("设置")]
        public static bool Remove(object key)
        {
            if (_redis.KeyExists(key.ToString()))
            {
                return _redis.KeyDelete(key.ToString());
            }
            return true;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Description("查询")]
        public static async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await _redis.StringGetAsync(key));
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Description("设置")]
        public static Task<bool> SetAsync<T>(object key, T value)
        {
            return _redis.StringSetAsync(key.ToString(), JsonConvert.SerializeObject(value));
        }
    }

    [Description("Redis客户端")]
    internal class RedisClient : IDisposable
    {
        //连接字符串
        private static readonly string _connectionString = AppSetting.Get("Redis.Configuration");
        //实例名称
        private static readonly string _instanceName = AppSetting.Get("Redis.InstanceName") ?? "default";
        //默认数据库
        private static readonly int _defaultDB = int.Parse(AppSetting.Get("Redis.DefaultDataBase") ?? "0");

        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connections = new();

        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private static ConnectionMultiplexer GetConnect()
            => _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase() => GetConnect().GetDatabase(_defaultDB);

        public static IServer GetServer(string configName = null!, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public static ISubscriber GetSubscriber(string configName = null!) => GetConnect().GetSubscriber();
        public void Dispose()
        {
            if (_connections != null && !_connections.IsEmpty)
                foreach (var item in _connections.Values)
                    item.Close();
        }
    }
}
