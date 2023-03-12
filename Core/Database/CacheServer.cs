using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 缓存
    /// </summary>
    [Description("缓存")]
    public class CacheServer
    {
        /// <summary>
        /// 用户Token缓存
        /// </summary>
        [Description("用户Token缓存")]
        private static Dictionary<string, CacheUser> UserConnections = new Dictionary<string, CacheUser>();

        /// <summary>
        /// 账户在线缓存
        /// </summary>
        [Description("账户在线缓存")]
        private static Dictionary<long, DateTime> OnlineCaches = new Dictionary<long, DateTime>();

        /// <summary>
        /// 用户请求记录时间缓存
        /// </summary>
        [Description("用户请求记录时间缓存")]
        private static Dictionary<string, HashSet<DateTime>> RequestTime = new Dictionary<string, HashSet<DateTime>>();

        /// <summary>
        /// 用户请求锁定时间缓存
        /// </summary>
        [Description("用户请求锁定时间缓存")]
        private static Dictionary<string, DateTime> RequestLockTime = new Dictionary<string, DateTime>();

        /// <summary>
        /// 日志数据缓存
        /// </summary>
        [Description("日志数据缓存")]
        private static Queue<CacheLogger> LogDatas = new Queue<CacheLogger>();

        #region User
        /// <summary>
        /// 查询当前用户
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Description("查询当前用户")]
        public static CacheUser Find(string key)
        {
            if (UserConnections.ContainsKey(key))
                return UserConnections[key];
            else
                return new CacheUser();
        }

        /// <summary>
        /// 设置当前用户
        /// </summary>
        [Description("设置当前用户")]
        public static void SetUser(string key, IdentityUser? user, string ip = "")
        {
            UserConnections[key] = user?.MapTo<CacheUser>() ?? new CacheUser();
            UserConnections[key].IP = ip;
        }

        /// <summary>
        /// Token连接
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Description("Token连接")]
        public static bool HasToken(string token)
        {
            return UserConnections.ContainsKey(token);
        }

        /// <summary>
        /// 在线
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aliveTime"></param>
        [Description("在线")]
        public static void Online(long id, DateTime? aliveTime)
        {
            if (!aliveTime.HasValue) 
                aliveTime = DateTime.Now;
             OnlineCaches[id] = aliveTime.Value;
        }

        /// <summary>
        /// 离线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Description("离线")]
        public static bool OffLine(long id)
        {
            lock (OnlineCaches)
            {
                if (OnlineCaches.ContainsKey(id) && OnlineCaches[id] <= DateTime.Now)
                {
                    OnlineCaches.Remove(id);
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Request
        /// <summary>
        /// 校验请求次数是否超限
        /// </summary>
        /// <param name="connnectionId"></param>
        /// <returns></returns>
        [Description("校验请求次数是否超限")]
        public static bool RecordRequestLimited(string connnectionId)
        {
            DateTime now = DateTime.Now;
            if (RequestTime.ContainsKey(connnectionId))
            {
                RequestTime[connnectionId] = RequestTime[connnectionId].Where(x => x >= now.AddSeconds(-5)).ToHashSet();
                RequestTime[connnectionId].Add(now);

                if (RequestTime[connnectionId].Count >= 20)
                {
                    RequestLockTime[connnectionId] = now.AddHours(1);
                    return false;
                }
            }
            else
                RequestTime[connnectionId] = new HashSet<DateTime> { now };

            return true;
        }

        /// <summary>
        /// 校验请求源是否限制
        /// </summary>
        /// <param name="connnectionId"></param>
        /// <returns></returns>
        [Description("校验请求源是否限制")]
        public static bool CheckRequestLocked(string connnectionId)
        {
            if (RequestLockTime.ContainsKey(connnectionId))
            {
                if (RequestLockTime[connnectionId] > DateTime.Now)
                {
                    RequestLockTime[connnectionId] = DateTime.Now.AddHours(1);
                    return true;
                }
                else
                    RequestLockTime.Remove(connnectionId);
            }
            return false;
        }
        #endregion

        #region Logger
        /// <summary>
        /// 推送日志
        /// </summary>
        [Description("推送日志")]
        public static void PushLogger(string path, string msg)
        {
            LogDatas.Enqueue(new CacheLogger { Path = path, LogMessage = msg });

            // Start Thread
            if (!logging)
                Task.Run(() => LogToFile());
        }

        /// <summary>
        /// 写入文件中
        /// </summary>
        [Description("写入文件中")]
        static bool logging = false;

        /// <summary>
        /// 写入文件
        /// </summary>
        [Description("写入文件")]
        static async Task LogToFile()
        {
            if (!logging)
            {
                logging = true;
                while (LogDatas.Any())
                {
                    var logData = LogDatas.Dequeue();
                    try
                    {
                        using (var sw = new StreamWriter(logData.Path, true))
                        {
                            await sw.WriteLineAsync(logData.LogMessage);
                        }
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(10);
                        PushLogger(logData.Path, logData.LogMessage);
                    }
                }
                
                logging = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 用户缓存
    /// </summary>
    [Description("用户缓存")]
    public class CacheUser
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public string Name { get; set; }

        /// <summary>
        /// 缓存IP
        /// </summary>
        [Description("缓存IP")]
        public string IP { get; set; }

        /// <summary>
        /// 用户当前关联组织ID
        /// </summary>
        [Description("用户当前关联组织ID")]
        public long TenantId { get; set; }

        /// <summary>
        /// 活跃更新时间
        /// </summary>
        [Description("活跃更新时间")]
        public DateTime AliveTime { get; set; }

        /// <summary>
        /// 活跃
        /// </summary>
        [Description("活跃")]
        public void Alive(int TimeOut = 5)
        {
            AliveTime = DateTime.Now;
            CacheServer.Online(Id, AliveTime);
            Task.Run(() =>
            {
                Thread.Sleep(1000 * TimeOut);

                if (CacheServer.OffLine(Id))
                {
                    DbScoped.SugarScope.Updateable<IdentityUser>(new { LastLogOut = AliveTime })
                    .Where(x => x.Id == Id)
                    .ExecuteCommandAsync();
                }
            });
        }
    }

    /// <summary>
    /// 待写入日志缓存
    /// </summary>
    [Description("待写入日志缓存")]
    public class CacheLogger
    {
        /// <summary>
        /// 日志目录
        /// </summary>
        [Description("日志目录")]
        public string Path { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        [Description("日志信息")]
        public string LogMessage { get; set; }
    }
}
