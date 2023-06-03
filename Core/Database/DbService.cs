using SqlSugar;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// MetaDB
    /// </summary>
    [Description("MetaDB")]
    internal class DbService : ISugarDB
    {
        /// <summary>
        /// ConnectionStrings
        /// </summary>
        [Description("ConnectionStrings")]
        private readonly string Connection = AppSetting.Get("ConnectionStrings.Default");

        /// <summary>
        /// DbType
        /// </summary>
        [Description("DbType")]
        private readonly string Type = AppSetting.Get("ConnectionStrings.DbType");

        /// <summary>
        /// ConnectionConfig
        /// </summary>
        [Description("ConnectionConfig")]
        private readonly ConnectionConfig _config;

        /// <summary>
        /// Scope
        /// </summary>
        [Description("Scope")]
        private readonly SqlSugarScope _scope;

        /// <summary>
        /// AppLogger
        /// </summary>
        [Description("AppLogger")]
        private readonly AppLogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        [Description("Constructor")]
        public DbService(AppLogger logger)
        {
            _logger = logger;
            _config = new ConnectionConfig()
            {
                DbType = (DbType)Enum.Parse(typeof(DbType), Type),
                ConnectionString = Connection,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            };
            _scope = new(_config, db => db.Aop.OnLogExecuting = (s, p) => ExecutingLog(s, p));
        }

        /// <summary>
        /// Client
        /// </summary>
        [Description("Client")]
        public SqlSugarClient Client => new(_config, db => db.Aop.OnLogExecuting = (s, p) => ExecutingLog(s, p));

        /// <summary>
        /// Scope
        /// </summary>
        [Description("Scope")]
        public SqlSugarScope Scope => _scope;

        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="sql">Sql</param>
        /// <param name="parms">Params</param>
        [Description("Logger")]
        void ExecutingLog(string sql, SugarParameter[] parms)
        {
            foreach (var pv in parms)
                sql = sql.Replace(pv.ParameterName, "\"" + pv.Value + "\"");

            _logger?.Db(sql);
        }
    }
}
