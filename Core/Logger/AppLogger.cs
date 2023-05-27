using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Logger
    /// </summary>
    [Description("Logger")]
    public class AppLogger
    {
        /// <summary>
        /// Cache
        /// </summary>
        [Description("Cache")]
        private readonly Queue<(string, string)> LogDatas = new();

        #region Logger
        /// <summary>
        /// Pusher
        /// </summary>
        [Description("Pusher")]
        void PushLogger(string path, string msg)
        {
            LogDatas.Enqueue(new (path, msg));

            // Start Thread
            if (!logging)
                Task.Run(() => LogToFile());
        }

        /// <summary>
        /// Is IO
        /// </summary>
        [Description("Is IO")]
        bool logging = false;

        /// <summary>
        /// IO to file
        /// </summary>
        [Description("IO to file")]
        async Task LogToFile()
        {
            if (!logging)
            {
                logging = true;
                while (LogDatas.Any())
                {
                    var logData = LogDatas.Dequeue();
                    try
                    {
                        using var sw = new StreamWriter(logData.Item1, true);
                        await sw.WriteLineAsync(logData.Item1);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(10);
                        PushLogger(logData.Item1, logData.Item2);
                    }
                }

                logging = false;
            }
        }
        #endregion

        /// <summary>
        /// Add to cache
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="method"></param>
        [Description("Add to cache")]
        void Log(string msg, string method)
        {
            var log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff") + "\n" + msg;
            Console.WriteLine(log);
            var basepath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(basepath, "Log", $"{String.Format(DateTime.Today.ToString("yyyy/MM/{0}/dd"), method)}.txt");
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            PushLogger(path, log);
            GC.Collect();
        }

        /// <summary>
        /// Record request
        /// </summary>
        /// <param name="log"></param>
        [Description("Record request")]
        public void Request(string log) => Task.Run(() => Log(log, "Request"));

        /// <summary>
        /// Record sqls
        /// </summary>
        /// <param name="log"></param>
        [Description("Record sqls")]
        public void Db(string log) => Task.Run(() => Log(log, "Db"));
    }
}
