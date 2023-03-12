using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 日志
    /// </summary>
    [Description("日志")]
    public class MetaLogger
    {
        /// <summary>
        /// 推送日志到缓存
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        [Description("推送日志到缓存")]
        static void Log(string msg, string method)
        {
            var log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff") + "\n" + msg;
            Console.WriteLine(log);
            var basepath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(basepath, "Log", $"{String.Format(DateTime.Today.ToString("yyyy/MM/{0}/dd"), method)}.txt");
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            CacheServer.PushLogger(path, log);
            GC.Collect();
        }

        /// <summary>
        /// 记录Http请求
        /// </summary>
        /// <param name="log"></param>
        [Description("记录Http请求")]
        public static void Request(string log)
        {
            Task.Run(() => Log(log, "Request"));
        }

        /// <summary>
        /// 记录数据操作语句
        /// </summary>
        /// <param name="log"></param>
        [Description("记录数据操作语句")]
        public static void Db(string log)
        {
            Task.Run(() => Log(log, "Db"));
        }
    }
}
