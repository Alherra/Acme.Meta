using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Encryp service
    /// </summary>
    public interface IEncrypter
    {
        /// <summary> 
        /// Encrypt data 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        string Encrypt(string Text, string sKey);

        /// <summary> 
        /// Decrypt data 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        string Decrypt(string Text, string sKey);
    }
}
