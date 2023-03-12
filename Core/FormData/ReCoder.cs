using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    public class ReCoder
    {
        /// <summary>
        /// 转化62位数
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public async static Task<string> Trans(long line)
        {
            StringBuilder sb = new();
            await Task.Run(() =>
            {
                List<byte> bytes = new();
                while (line > 62)
                {
                    byte c = (byte)(line % 62);
                    bytes.Add(c);
                    line /= 62;
                }
                bytes.Add((byte)line);
                foreach (var item in bytes)
                {
                    var ascii = item + 48;
                    if (ascii > 57)
                    {
                        ascii += 7;
                    }
                    if (ascii > 90)
                    {
                        ascii += 6;
                    }
                    sb.Append((char)ascii);
                }
            });
            return sb.ToString();
        }

        /// <summary>
        /// 解析62位数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async static Task<long> ToNumber(string code)
        {
            long result = 0;
            await Task.Run(() =>
            {
                var chars = code.ToCharArray();
                var bit = 1;
                foreach (var item in chars)
                {
                    var val = Convert.ToByte(item);
                    if (val > 96)
                    {
                        val -= 6;
                    }
                    if (val > 64)
                    {
                        val -= 7;
                    }
                    val -= 48;
                    var index = bit - 1;
                    long bitval = 1;
                    while (index > 0)
                    {
                        bitval *= 62;
                        index--;
                    }
                    result += val * bitval;
                    bit++;
                }
            });
            return result;
        }
    }
}
