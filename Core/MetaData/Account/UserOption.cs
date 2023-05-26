using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Option policy
    /// </summary>
    [SugarTable("UserOption", tableDescription: "Option policy")]
    public class UserOption : Meta<long>
    {
        /// <summary>
        /// UserId
        /// </summary>
        [SugarColumn(ColumnName = "UserId", ColumnDescription = "UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Policy
        /// </summary>
        [SugarColumn(ColumnName = "Policy", ColumnDescription = "Policy")]
        public string Policy { get; set; } = String.Empty;
    }
}
