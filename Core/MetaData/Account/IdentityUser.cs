using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// User acount
    /// </summary>
    [SugarTable("T_System_Account", tableDescription: "User acount")]
    public class IdentityUser : AuditModel, IMultiTenant
    {
        /// <summary>
        /// UserName
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "UserName")]
        [Description("UserName")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "Password")]
        [Description("Password")]
        public virtual string Password { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "E-mail")]
        [Description("E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [SugarColumn(Length = 16, ColumnDescription = "PhoneNumber")]
        [Description("PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Enable
        /// </summary>
        [SugarColumn(ColumnDescription = "Enable")]
        [Description("Enable")]
        public virtual bool IsEnable { get; set; }

        /// <summary>
        /// OnlineTime
        /// </summary>
        [SugarColumn(ColumnDescription = "OnlineTime", IsNullable = true)]
        [Description("OnlineTime")]
        public virtual DateTime LastLogOut { get; set; }

        /// <summary>
        /// Id of the related tenant.
        /// </summary>
        [SugarColumn(ColumnDescription = "TenantId", IsNullable = true)]
        [Description("Id of the related tenant.")]
        public int TenantId { get; set; }
    }
}
