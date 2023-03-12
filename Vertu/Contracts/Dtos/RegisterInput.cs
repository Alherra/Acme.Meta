using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Account
{
    /// <summary>
    /// 注册信息
    /// </summary>
    [Description("注册信息")]
    public class RegisterInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string Password { get; set; } = String.Empty;

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Description("电子邮件")]
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        public string PhoneNumber { get; set; } = String.Empty;
    }
}
