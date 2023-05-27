using SqlSugar;
using System.Account;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// UserManagement
    /// </summary>
    [Description("UserManagement")]
    internal class AccountService : AppService, IAccountService
    {
        private readonly IEncrypter _encrypter;

        public AccountService(IEncrypter encrypter)
        {
            _encrypter = encrypter;
        }

        [Description("SignIn")]
        public Task<IdentityUser> SignIn(string account, string pwd)
            => AppDB.Execute(async db =>
            {
                var password = _encrypter.Encrypt(pwd, "^" + account);
                var user = await db.Queryable<IdentityUser>().SingleAsync(x => x.UserName == account);
                if (!user.Password.Equals(password))
                    throw new ArgumentException(message: "Unknown Password with Account");

                return user;
            });

        [Description("Register")]
        public Task<bool> Register(RegisterInput user)
            => AppDB.Execute(async db =>
            {
                if (CurrentUser != null && CurrentUser.Id > 0)
                    throw new ArgumentException(message: "Already logged!");
                
                if (await db.Queryable<IdentityUser>().AnyAsync(x => x.UserName == user.UserName))
                    throw new BussinessException(message: "Account Existed");

                user.Password = _encrypter.Encrypt(user.Password, "^" + user.UserName);
                return await db.Insert(user.MapTo<IdentityUser>());
            });

        [Description("UserInfo")]
        public Task<IdentityUser> GetUserAsync(string username)
            => AppDB.Execute(async db => await db.Queryable<IdentityUser>().SingleAsync(x => x.UserName == username));
    }
}
