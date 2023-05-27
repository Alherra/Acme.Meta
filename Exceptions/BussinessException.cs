using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Normal Error
    /// </summary>
    [Description("Normal Error")]
    public class BussinessException : Exception
    {
        /// <summary>
        /// Normal Error
        /// </summary>
        /// <param name="message"></param>
        [Description("Normal Error")]
        public BussinessException(string message) : base(message) { }

        /// <summary>
        /// Normal Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("Normal Error")]
        public BussinessException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
