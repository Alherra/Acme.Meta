using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    public static class Email
    {
        //static string Email_Account = "";
        //static string Email_Password = "";
        //static string Email_Host = "smtp.qq.com";

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="recipient">destination</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public async static Task SendEmail(string recipient, string subject, string body)
        {
            #region Configuration
            string Email_Account = string.Empty, Email_Password = string.Empty, Email_Host = string.Empty;
            await Task.Run(() =>
            {
                Email_Account = AppSetting.Get("Email.Account");
                Email_Password = AppSetting.Get("Email.Password");
                Email_Host = AppSetting.Get("Email.Host");
            });
            #endregion

            #region Mail Message
            MailMessage mailMessage = new();

            mailMessage.From = new MailAddress(Email_Account);
            mailMessage.To.Add(new MailAddress(recipient));

            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;

            mailMessage.Body = body;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            #endregion

            #region Send
            SmtpClient smtp = new();
            smtp.Host = Email_Host;
            smtp.Credentials = new NetworkCredential(Email_Account, Email_Password);

            await smtp.SendMailAsync(mailMessage);
            #endregion
        }
    }
}
