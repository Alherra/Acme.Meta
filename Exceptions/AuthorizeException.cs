﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Authorize Error
    /// </summary>
    [Description("Authorize Error")]
    public class AuthorizeException : Exception
    {
        /// <summary>
        /// Authorize Error
        /// </summary>
        /// <param name="message"></param>
        [Description("Authorize Error")]
        public AuthorizeException(string message) : base(message) { }

        /// <summary>
        /// Authorize Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("Authorize Error")]
        public AuthorizeException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
