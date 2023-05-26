using System.Account;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 用户资源服务接口
    /// </summary>
    [Description("用户资源服务接口")]
    public interface IAccountService
    {
        /// <summary>
        /// SignIn
        /// </summary>
        [Description("SignIn")]
        Task<IdentityUser> SignIn(string account, string pwd);

        /// <summary>
        /// Register
        /// </summary>
        [Description("Register")]
        Task<bool> Register(RegisterInput user);

        /// <summary>
        /// UserInfo
        /// </summary>
        [Description("UserInfo")]
        Task<IdentityUser> GetUserAsync(string username);
    }
}
