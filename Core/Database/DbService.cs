using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 数据服务
    /// </summary>
    [Description("数据服务")]
    public class DbService
    {
        /// <summary>
        /// 连接字符
        /// </summary>
        [Description("连接字符")]
        private readonly static string Connection = AppSetting.Get("ConnectionStrings.Default");

        /// <summary>
        /// 库类型
        /// </summary>
        [Description("库类型")]
        private readonly static string Type = AppSetting.Get("ConnectionStrings.DbType");

        /// <summary>
        /// 客户端
        /// </summary>
        [Description("客户端")]
        public static SqlSugarClient Client => new(new ConnectionConfig()
        {
            ConnectionString = Connection,
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });

        /// <summary>
        /// 客户端
        /// </summary>
        [Description("客户端")]
        public static readonly SqlSugarScope Db = new(new ConnectionConfig() 
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            ConnectionString = Connection,
            IsAutoCloseConnection = true
        },
            db =>
            {
                //如果用单例配置要统一写在这儿
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    ExecutingLog(s, p);
                };
            });

        /// <summary>
        /// 查询专用引擎
        /// </summary>
        [Description("查询专用引擎")]
        public static SqlSugarScope QueryScope => new(new ConnectionConfig()
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            ConnectionString = Connection,
            IsAutoCloseConnection = true
        },
            db =>
            {
                //如果用单例配置要统一写在这儿
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    ExecutingLog(s, p);
                };
            });

        /// <summary>
        /// ABP查询专用引擎
        /// </summary>
        [Description("ABP查询专用引擎")]
        public static SqlSugarScope Abp => new(new ConnectionConfig()
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), AppSetting.Get("ConnectionStrings.AbpType")),
            ConnectionString = AppSetting.Get("ConnectionStrings.ABP"),
            IsAutoCloseConnection = true
        },
            db =>
            {
                //如果用单例配置要统一写在这儿
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    ExecutingLog(s, p);
                };
            });

        /// <summary>
        /// 语句日志
        /// </summary>
        /// <param name="s">Sql</param>
        /// <param name="p">Params</param>
        [Description("语句日志")]
        static void ExecutingLog(string s, SugarParameter[] p)
        {
            foreach (var pv in p)
                s = s.Replace(pv.ParameterName, "\"" + pv.Value + "\"");

            MetaLogger.Db(s);
        }
    }
}
