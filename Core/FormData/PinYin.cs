using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class PinYin
    {
        /// <summary>
        /// 汉字转化为拼音
        /// </summary>
        /// <param name="str">汉字</param>
        /// <returns>全拼</returns>
        public static async Task<string> ConvertAsync(string str)
        {
            StringBuilder pinyin = new();
            await Task.Run(() => {
                foreach (char obj in str)
                {
                    try
                    {
                        ChineseChar chineseChar = new(obj);
                        string t = chineseChar.Pinyins[0].ToString();
                        pinyin.Append(t[0..^1]);
                    }
                    catch
                    {
                        pinyin.Append(obj);
                    }
                }
            });
            return pinyin.ToString();
        }


        /// <summary>
        /// 汉字转化为拼音
        /// </summary>
        /// <param name="str">汉字</param>
        /// <returns>全拼</returns>
        public static string Convert(string str)
        {
            StringBuilder pinyin = new();

            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    pinyin.Append(t[0..^1]);
                }
                catch
                {
                    pinyin.Append(obj);
                }
            }
            return pinyin.ToString();
        }
    }
}
