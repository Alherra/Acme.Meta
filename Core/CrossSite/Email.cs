using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal class Email : IEmail
    {
        private readonly string Email_Account;
        private readonly string Email_Password;
        private readonly string Email_Host;

        public Email()
        {
            Email_Account = AppSetting.Get("Email.Account");
            Email_Password = AppSetting.Get("Email.Password");
            Email_Host = AppSetting.Get("Email.Host");
        }

        /// <summary>
        /// Send e-mail.
        /// </summary>
        /// <param name="recipient">destination</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public async Task SendEmail(IEnumerable<string> recipients, string subject, string body)
        {
            #region Mail Message
            MailMessage mailMessage = new()
            {
                From = new MailAddress(Email_Account),
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
            };
            foreach (var recipient in recipients) mailMessage.To.Add(new MailAddress(recipient));

            #endregion

            #region Send
            SmtpClient smtp = new()
            {
                Host = Email_Host,
                Credentials = new NetworkCredential(Email_Account, Email_Password)
            };

            await smtp.SendMailAsync(mailMessage);
            #endregion
        }
    }
}
