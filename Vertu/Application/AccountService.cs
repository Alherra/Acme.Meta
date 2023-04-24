using SqlSugar;
using System.Account;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// 用户资源服务
    /// </summary>
    [Description("用户资源服务")]
    internal class AccountService : AppService, IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        public Task<IdentityUser> Login(string account, string pwd)
            => AppDB.Execute(async db =>
            {
                if (CurrentUser != null && CurrentUser.Id > 0)
                    throw new ArgumentException(message: "Already logged!");

                var password = Encrypter.EncryptMD5(pwd, "^" + account);
                var user = await db.Queryable<IdentityUser>().SingleAsync(x => x.UserName == account);
                if (!user.Password.Equals(password))
                    throw new ArgumentException(message: "Unknown Password with Account");

                return user;
            });

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        public Task<bool> Register(RegisterInput user)
            => AppDB.Execute(async db =>
            {
                if (CurrentUser != null && CurrentUser.Id > 0)
                    throw new ArgumentException(message: "Already logged!");
                
                if (await db.Queryable<IdentityUser>().AnyAsync(x => x.UserName == user.UserName))
                    throw new BussinessException(message: "Account Existed");

                user.Password = Encrypter.EncryptMD5(user.Password, "^" + user.UserName);
                return await db.Insert(user.MapTo<IdentityUser>());
            });
        

        /// <summary>
        /// 获取用户
        /// </summary>
        [Description("获取用户")]
        public Task<IdentityUser> GetUserAsync(string username)
            => AppDB.Execute(async db => await db.Queryable<IdentityUser>().SingleAsync(x => x.UserName == username));

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
