using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    public static class RandomBuilder
    {
        /// <summary>
        /// 生成单个随机数字
        /// </summary>
        static int CreateNum()
        {
            Random random = new(Guid.NewGuid().GetHashCode());
            int num = random.Next(10);
            return num;
        }

        /// <summary>
        /// 生成单个大写随机字母
        /// </summary>
        static string CreateUpper()
        {
            //A-Z的 ASCII值为65-90
            Random random = new(Guid.NewGuid().GetHashCode());
            int num = random.Next(65, 91);
            string letter = Convert.ToChar(num).ToString();
            return letter;
        }

        /// <summary>
        /// 生成单个小写随机字母
        /// </summary>
        static string CreateLower()
        {
            //a-z的 ASCII值为97-122
            Random random = new(Guid.NewGuid().GetHashCode());
            int num = random.Next(97, 123);
            string letter = Convert.ToChar(num).ToString();
            return letter;
        }


        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Creator(int length)
        {
            // 创建一个StringBuilder对象存储密码
            StringBuilder sb = new();
            //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
            for (int i = 0; i < length; i++)
            {
                Random random = new(Guid.NewGuid().GetHashCode());
                //随机选择里面其中的一种字符生成
                switch (random.Next(3))
                {
                    case 0:
                        //调用生成生成随机数字的方法
                        sb.Append(CreateNum());
                        break;
                    case 1:
                        //调用生成生成随机小写字母的方法
                        sb.Append(CreateLower());
                        break;
                    case 2:
                        //调用生成生成随机大写字母的方法
                        sb.Append(CreateUpper());
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// 数字
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Number(int length)
        {
            // 创建一个StringBuilder对象存储密码
            StringBuilder sb = new();
            //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
            for (int i = 0; i < length; i++)
            {
                sb.Append(CreateNum());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// 字母
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Letter(int length)
        {
            // 创建一个StringBuilder对象存储密码
            StringBuilder sb = new();
            //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
            for (int i = 0; i < length; i++)
            {
                Random random = new(Guid.NewGuid().GetHashCode());
                //随机选择里面其中的一种字符生成
                switch (random.Next(2))
                {
                    case 0:
                        //调用生成生成随机小写字母的方法
                        sb.Append(CreateLower());
                        break;
                    case 1:
                        //调用生成生成随机大写字母的方法
                        sb.Append(CreateUpper());
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// 大写
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Upper(int length)
        {
            // 创建一个StringBuilder对象存储密码
            StringBuilder sb = new();
            //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
            for (int i = 0; i < length; i++)
            {
                sb.Append(CreateUpper());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机字符串
        /// 大写
        /// </summary>
        /// <param name="length">字符串的长度</param>
        /// <returns></returns>
        public static string Lower(int length)
        {
            // 创建一个StringBuilder对象存储密码
            StringBuilder sb = new();
            //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
            for (int i = 0; i < length; i++)
            {
                sb.Append(CreateLower());
            }
            return sb.ToString();
        }
    }
}
