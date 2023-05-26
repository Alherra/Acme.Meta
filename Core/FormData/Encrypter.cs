using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public class Encrypter : IEncrypter
    {
        #region ========MD5 Encrypt & Decrypt========

        public string Encrypt(string Text, string sKey)
        {
            using DESCryptoServiceProvider des = new();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey)[..8]);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey)[..8]);
            System.IO.MemoryStream ms = new();
            CryptoStream cs = new(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }


        public string Decrypt(string Text, string sKey)
        {
            try
            {
                using DESCryptoServiceProvider des = new();
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
                des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey)[..8]);
                des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey)[..8]);
                System.IO.MemoryStream ms = new();
                CryptoStream cs = new(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
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
        public string Md5Hash(string input)
        {
            using MD5CryptoServiceProvider md5Hasher = new();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion
    }
}
