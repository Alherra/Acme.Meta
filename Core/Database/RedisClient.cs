using Meta;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace System
{
    [Description("RedisClient")]
    internal class RedisClient : IRedis, IDisposable
    {
        private static RedisClient _instance = null!;

        private RedisClient() { }

        public static RedisClient Instance => _instance ??= new RedisClient();

        //连接字符串
        private readonly string _connectionString = AppSetting.Get("Redis.Configuration");
        //实例名称
        private readonly string _instanceName = AppSetting.Get("Redis.InstanceName") ?? "default";
        //默认数据库
        private readonly int _defaultDB = int.Parse(AppSetting.Get("Redis.DefaultDataBase") ?? "0");

        private readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connections = new();

        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
            => _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase RedisDB => GetConnect().GetDatabase(_defaultDB);

        public IServer GetServer(int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null!) => GetConnect().GetSubscriber(configName);

        public void Dispose()
        {
            if (_connections != null && !_connections.IsEmpty)
                foreach (var item in _connections.Values)
                    item.Close();
        }

        public static async Task<T> Get<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(await Instance.RedisDB.StringGetAsync(key)) ?? default!;
            }
            catch (Exception)
            {
                return default!;
            }
        }

        public static Task<bool> Set<T>(string key, T val, int expireSecond = -1)
            => Instance.RedisDB.StringSetAsync(key, JsonConvert.SerializeObject(val), expireSecond == -1 ? null : TimeSpan.FromSeconds(expireSecond));
    }
}
