using System.ComponentModel;

namespace Meta.Account
{
    /// <summary>
    /// 用户资源服务
    /// </summary>
    [Description("用户资源服务")]
    public class AccountService : MetaService, IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        public async Task<IdentityUser> Login(string account ,string pwd)
        {
            if (CurrentUser != null && CurrentUser.Id > 0)
                throw new ArgumentException(message: "Already logged!");

            var password = Encrypter.EncryptMD5(pwd, "^" + account);
            var user = await Queryable<IdentityUser>().SingleAsync(x => x.UserName == account);
            if (!user.Password.Equals(password))
                throw new ArgumentException(message: "Unknown Password with Account");

            return user;
        }

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        public async Task<bool> Register(RegisterInput user)
        {
            if (CurrentUser != null && CurrentUser.Id > 0)
                throw new ArgumentException(message: "Already logged!");

            if (await Queryable<IdentityUser>().AnyAsync(x => x.UserName == user.UserName))
                throw new BussinessException(message: "Account Existed");

            user.Password = Encrypter.EncryptMD5(user.Password, "^" + user.UserName);
            return (await Insertor(user.MapTo<IdentityUser>())) == 1;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        [Description("获取用户")]
        public async Task<IdentityUser> GetUserAsync(string username)
        {
            return await Queryable<IdentityUser>().SingleAsync(x => x.UserName == username);
        }

        /// <summary>
        /// 活跃连接
        /// </summary>
        [Description("活跃连接")]
        public async Task<bool> KeepAliveInterval()
        {
            if (CurrentUser is null || CurrentUser.Id <= 0)
                throw new ArgumentException(message: "Not logged!");

            await Task.Run(() => CurrentUser.Alive(20));
            return true;
        }
    }
}
