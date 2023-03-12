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
    /// 账户
    /// </summary>
    [SugarTable("Account", tableDescription: "账户")]
    public class IdentityUser : Meta<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "用户名")]
        [Description("用户名")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "密码")]
        [Description("密码")]
        public virtual string Password { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "电子邮件")]
        [Description("电子邮件")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(Length = 16, ColumnDescription = "手机号码")]
        [Description("手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 上次在线时间
        /// </summary>
        [SugarColumn(ColumnDescription = "上次在线时间", IsNullable = true)]
        [Description("上次在线时间")]
        public virtual DateTime LastLogOut { get; set; }
    }
}
