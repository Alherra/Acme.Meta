using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 无效参数异常
    /// </summary>
    [Description("无效参数异常")]
    internal class InvalidArgumentException : Exception
    {
        /// <summary>
        /// 无效参数异常
        /// </summary>
        /// <param name="message"></param>
        [Description("无效参数异常")]
        public InvalidArgumentException(string message) : base(message) { }

        /// <summary>
        /// 无效参数异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("无效参数异常")]
        public InvalidArgumentException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
