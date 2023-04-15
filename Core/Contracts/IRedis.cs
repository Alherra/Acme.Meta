using StackExchange.Redis;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// Redis client for app.
    /// </summary>
    [Description("Redis")]
    public interface IRedis
    {
        IDatabase RedisDB { get; }

        IServer GetServer(int endPointsIndex = 0);

        ISubscriber GetSubscriber(string configName = null!);
    }
}
