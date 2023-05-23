using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface ITencentSMS
    {
        /// <summary>
        /// Send sms.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="phoneNumber">phonenumber (without +86)</param>
        /// <param name="template"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task Send(string message, ulong phoneNumber, SendTemplate template, ushort state = 86);
    }
}
