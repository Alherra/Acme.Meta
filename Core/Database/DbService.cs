using SqlSugar;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// MetaDB
    /// </summary>
    [Description("MetaDB")]
    public class DbService
    {
        /// <summary>
        /// ConnectionStrings
        /// </summary>
        [Description("ConnectionStrings")]
        private readonly static string Connection = AppSetting.Get("ConnectionStrings.Default");

        /// <summary>
        /// DbType
        /// </summary>
        [Description("DbType")]
        private readonly static string Type = AppSetting.Get("ConnectionStrings.DbType");

        /// <summary>
        /// Client
        /// </summary>
        [Description("Client")]
        public static SqlSugarClient Client => new(new ConnectionConfig()
        {
            ConnectionString = Connection,
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        }, db => db.Aop.OnLogExecuting = (s, p) => ExecutingLog(s, p));

        /// <summary>
        /// Scope
        /// </summary>
        [Description("Scope")]
        public static readonly SqlSugarScope Db = new(new ConnectionConfig() 
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            ConnectionString = Connection,
            IsAutoCloseConnection = true
        }, db => db.Aop.OnLogExecuting = (s, p) => ExecutingLog(s, p));

        /// <summary>
        /// Engine
        /// </summary>
        [Description("Engine")]
        public static SqlSugarScope QueryScope => new(new ConnectionConfig()
        {
            DbType = (DbType)Enum.Parse(typeof(DbType), Type),
            ConnectionString = Connection,
            IsAutoCloseConnection = true
        }, db => db.Aop.OnLogExecuting = (s, p) => ExecutingLog(s, p));

        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="s">Sql</param>
        /// <param name="p">Params</param>
        [Description("Logger")]
        static void ExecutingLog(string s, SugarParameter[] p)
        {
            foreach (var pv in p)
                s = s.Replace(pv.ParameterName, "\"" + pv.Value + "\"");

            var logger = ServiceProvider.GetService<AppLogger>();
            if (logger != null) logger.Db(s);
        }
    }
}
