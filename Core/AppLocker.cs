using Microsoft.Extensions.DependencyInjection;
using Meta;

namespace System
{
    /// <summary>
    /// 应用锁
    /// </summary>
    public abstract class AppLocker : IDisposable
    {
        /// <summary>
        /// Bool锁
        /// 
        /// (Redis 键名)
        /// </summary>
        private readonly static string redisLockKey = "Locks:";

        /// <summary>
        /// Redis 锁实例对象
        /// </summary>
        private sealed class RedisLocker : AppLocker, IDisposable
        {
            /// <summary>
            /// Redis 锁键
            /// </summary>
            private readonly string redisKey;

            /// <summary>
            /// Redis 锁实例对象
            /// </summary>
            /// <param name="key">锁的键名</param>
            /// <param name="rollBack">占用是否退回</param>
            public RedisLocker(string key, bool rollBack)
            {
                redisKey = redisLockKey + key;

                // 冲突性质的操作抛出异常停止
                if (RedisClient.Get<bool>(redisKey).Result)
                {
                    if (rollBack)
                    {
                        throw new Exception("Operation key value is processing locked.");
                    }

                    // Redis 等待锁
                    while (RedisClient.Get<bool>(redisKey).Result)
                    {
                        Thread.Sleep(100);
                    }
                }

                // Redis 加锁
                RedisClient.Set(redisKey, true, 120);
            }

            /// <summary>
            /// 释放锁
            /// </summary>
            public override async void Dispose()
            {
                // Redis 解锁
                await RedisClient.Set(redisKey, false, 10);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 创建一个锁
        /// </summary>
        /// <param name="key">锁的键名</param>
        /// <param name="rollBack">占用是否退回</param>
        /// <returns></returns>
        public static AppLocker CreateLock(string key, bool rollBack = false)
        {
            return new RedisLocker(key, rollBack);
        }


        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
