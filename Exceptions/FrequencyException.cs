using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Frequency Error
    /// </summary>
    [Description("Frequency Error")]
    public class FrequencyException : Exception
    {
        /// <summary>
        /// Frequency Error
        /// </summary>
        /// <param name="message"></param>
        [Description("Frequency Error")]
        public FrequencyException(string message) : base(message) { }

        /// <summary>
        /// Frequency Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        [Description("Frequency Error")]
        public FrequencyException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
