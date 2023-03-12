using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Meta
{
    public class Encrypter
    {

        #region ========加密========
        private static string EncryptDES(string input, string key)
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(input); var tripleDES = TripleDES.Create(); var byteKey = Encoding.UTF8.GetBytes(key); byte[] allKey = new byte[24]; Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor(); 
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length); 
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #region ========解密========
        private static string DecryptDES(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input); var tripleDES = TripleDES.Create(); var byteKey = Encoding.UTF8.GetBytes(key); byte[] allKey = new byte[24]; Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor(); 
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length); 
            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion

        #region ========MD5 Encrypt & Decrypt========

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string EncryptMD5(string Text, string sKey)
        {
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(Text);
                des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
#pragma warning restore SYSLIB0021 // 类型或成员已过时
        }


        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DecryptMD5(string Text, string sKey)
        {
            try
            {
#pragma warning disable SYSLIB0021 // 类型或成员已过时
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    int len;
                    len = Text.Length / 2;
                    byte[] inputByteArray = new byte[len];
                    int x, i;
                    for (x = 0; x < len; x++)
                    {
                        i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                        inputByteArray[x] = (byte)i;
                    }

                    //参数str类型是string
                    // 注：.NET Core 不支持System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");轉為函數Md5Hash(string input)
                    des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
                    des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.Default.GetString(ms.ToArray());
                }
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
#pragma warning restore SYSLIB0021 // 类型或成员已过时
        }
        #endregion
    }
}
