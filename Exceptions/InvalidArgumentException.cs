using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// InvalidArgument
    /// </summary>
    [Description("InvalidArgument")]
    internal class InvalidArgumentException : Exception
    {
        /// <summary>
        /// InvalidArgument
        /// </summary>
        /// <param name="message"></param>
        [Description("InvalidArgument")]
        public InvalidArgumentException(string message) : base(message) { }

        /// <summary>
        /// InvalidArgument
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("InvalidArgument")]
        public InvalidArgumentException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
