using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 业务逻辑异常
    /// </summary>
    [Description("业务逻辑异常")]
    public class BussinessException : Exception
    {
        /// <summary>
        /// 业务逻辑异常
        /// </summary>
        /// <param name="message"></param>
        [Description("业务逻辑异常")]
        public BussinessException(string message) : base(message) { }

        /// <summary>
        /// 业务逻辑异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("业务逻辑异常")]
        public BussinessException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
