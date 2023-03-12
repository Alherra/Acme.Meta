using Meta.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 用户资源服务接口
    /// </summary>
    [Description("用户资源服务接口")]
    public interface IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Task<IdentityUser> Login(string account, string pwd);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Description("注册")]
        Task<bool> Register(RegisterInput user);

        /// <summary>
        /// 获取用户
        /// </summary>
        [Description("获取用户")]
        Task<IdentityUser> GetUserAsync(string username);

        /// <summary>
        /// 活跃连接
        /// </summary>
        [Description("活跃连接")]
        Task<bool> KeepAliveInterval();
    }
}
