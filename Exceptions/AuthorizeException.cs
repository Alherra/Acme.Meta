using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 身份验证异常
    /// </summary>
    [Description("身份验证异常")]
    public class AuthorizeException : Exception
    {
        /// <summary>
        /// 身份验证异常
        /// </summary>
        /// <param name="message"></param>
        [Description("身份验证异常")]
        public AuthorizeException(string message) : base(message) { }

        /// <summary>
        /// 身份验证异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("身份验证异常")]
        public AuthorizeException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
