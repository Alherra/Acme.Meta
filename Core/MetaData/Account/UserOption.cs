using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 账户权限
    /// </summary>
    [SugarTable("UserOption", tableDescription: "账户权限")]
    public class UserOption : Meta<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "UserId", ColumnDescription = "UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        [SugarColumn(ColumnName = "Action", ColumnDescription = "Action")]
        public string Action { get; set; } = String.Empty;
    }
}
