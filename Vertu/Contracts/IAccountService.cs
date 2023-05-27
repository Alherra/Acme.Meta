using System.Account;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// UserManagement
    /// </summary>
    [Description("UserManagement")]
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
    }
}
