using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Core.MetaData.Account
{
    /// <summary>
    /// User orgnization
    /// </summary>
    [SugarTable("T_System_Tenant", tableDescription: "Orgnization")]
    public class Tenant : AuditModel
    {
        /// <summary>
        /// Tenant name
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "Tenant name")]
        [Description("TenantName")]
        public virtual string TenantName { get; set; }

        /// <summary>
        /// Manager
        /// </summary>
        [SugarColumn(ColumnDescription = "Manager")]
        [Description("ManagerID")]
        public virtual int ManagerID { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [SugarColumn(ColumnDescription = "Description")]
        [Description("Description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Enable
        /// </summary>
        [SugarColumn(ColumnDescription = "Enable")]
        [Description("Enable")]
        public virtual bool IsEnable { get; set; }
    }
}
