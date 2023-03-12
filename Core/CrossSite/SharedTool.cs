using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration.Json;
using static Google.Protobuf.WellKnownTypes.Field;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.ComponentModel;
using System.Speech.Synthesis;

namespace Meta
{
    public class SharedTool : IDisposable
    {
        // obtains user token       
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser       
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEWCREDENTIALS = 9;//域控中的需要用:Interactive = 2       
        private bool disposed;

        public SharedTool(string username, string password, string ip)
        {
            // initialize tokens       
            IntPtr pExistingTokenHandle = new(0);
            IntPtr pDuplicateTokenHandle = new(0);

            try
            {
                // get handle to token       
                bool bImpersonated = LogonUser(username, ip, password,
                    LOGON32_LOGON_NEWCREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)       
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Save Json File
        /// 
        /// /Upload/Json/file.json
        /// </summary>
        /// <param name="file">File Path</param>
        /// <param name="json">Json Data</param>
        /// <returns></returns>
        public async static Task<bool> SaveJson(string file, string json)
        {
            string Admin = string.Empty,
                Pwd = string.Empty, 
                Host = string.Empty;
            try
            {
                #region Get Configuration
                await Task.Run(() =>
                {
                    Admin = AppSetting.Get("Shared.Admin");
                    Pwd = AppSetting.Get("Shared.Pwd");
                    Host = AppSetting.Get("Shared.Host");
                });
                #endregion
                #region Save Json File
                await Task.Run(() =>
                {
                    using SharedTool tool = new(Admin, Pwd, Host);
                    string selectPath = $"{@"\\"}{Host}\\Upload\\QR";

                    var path = Path.Combine(selectPath, $"{file}.json");
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir!);

                    using StreamWriter stream = new(path);
                    stream.WriteLine(json);
                    stream.Flush();
                    stream.Close();
                });
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }


    public static class ShareFile
    {
        static string Admin = string.Empty;
        static string Pwd = string.Empty;
        static string Host = string.Empty;
        static string HttpAddress = string.Empty;
        static string Port = string.Empty;



        /// <summary>
        /// Save Json File
        /// 
        /// /Upload/Json/file.json
        /// </summary>
        /// <param name="file">File Path</param>
        /// <param name="json">Json Data</param>
        /// <returns></returns>
        [Description("保存Json")]
        public static async Task<bool> SaveJson(string file, string json)
        {
            try
            {
                #region Get Configuration
                await Task.Run(() =>
                {
                    Admin = AppSetting.Get("Shared.Admin");
                    Pwd = AppSetting.Get("Shared.Pwd");
                    Host = AppSetting.Get("Shared.Host");
                });
                #endregion
                #region Save Json File
                await Task.Run(() =>
                {
                    using SharedTool tool = new(Admin, Pwd, Host);
                    string selectPath = $"{@"\\"}{Host}\\Upload\\QR";

                    var path = Path.Combine(selectPath, $"{file}.json");
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir!);

                    using StreamWriter stream = new(path);
                    stream.WriteLine(json);
                    stream.Flush();
                    stream.Close();
                });
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        [Description("保存图片")]
        public static async Task<string> SaveImage(string base64Str)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string[] str = base64Str.Split(',');
                byte[] imageBytes = Convert.FromBase64String(str[1]);
                //读入MemoryStream对象
                MemoryStream memoryStream = new(imageBytes, 0, imageBytes.Length);
                memoryStream.Write(imageBytes, 0, imageBytes.Length);

                string ext = '.' + base64Str.Split(',')[0].Split(';')[0].Split('/')[1];
                string selectPath = $"{@"\\"}{Host}\\Upload\\Image";
                return await SaveToShare(imageBytes, ext, selectPath, (filepath) =>
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        using Image image = Image.FromStream(memoryStream);
                        // 共享存储
                        image.Save(filepath); //将图片存储Server.MapPath("pic\\") + iname + "." + hz
                    }
                });
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Description("保存文件")]
        public static async Task<string> SaveFile(IFormFile file)
        {
            string selectPath = $"{@"\\"}{Host}\\Upload\\File";

            //保存文件
            var stream = file.OpenReadStream();
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];

            var ext = Path.GetExtension(file.FileName);
            return await SaveToShare(bytes, ext, selectPath, async (filepath) =>
            {
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始 
                stream.Seek(0, SeekOrigin.Begin);
                // 把 byte[] 写入文件 
                FileStream fs = new(filepath, FileMode.OpenOrCreate);
                await stream.CopyToAsync(fs);
                //BinaryWriter bw = new BinaryWriter(fs);
                //bw.Write(bytes);
                //bw.Close();
                //fs.Close();
            });
        }

        /// <summary>
        /// 生成语音
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        [Description("生成语音")]
        public static async Task<string> SynthesizeAudioAsync(string words)
        {
            string path = string.Empty;
            await SaveAsync(() =>
            {
                path = $"{@"\\"}{Host}\\Upload\\File\\Wav\\{words}.wav";
                if (File.Exists(path))
                    return;

                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir!);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Windows 相关逻辑
                    using SpeechSynthesizer speechSynthesizer = new();
                    speechSynthesizer.SetOutputToWaveFile(path);
                    speechSynthesizer.Speak(words);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Linux 相关逻辑
                    //var config = SpeechConfig.FromSubscription("<paste-your-speech-key-here>", "<paste-your-speech-location/region-here>");
                    //// Set either the `SpeechSynthesisVoiceName` or `SpeechSynthesisLanguage`.
                    //config.SpeechSynthesisLanguage = "zh-CN";
                    //config.SpeechSynthesisVoiceName = "zh-CN-XiaoxuanNeural";
                    throw new BussinessException("当前方法仅支持Windows平台项目");
                }
            });
            return "http:" + path.Replace('\\', '/').Replace(Host, HttpAddress).Replace("/Upload", Port);
        }

        /// <summary>
        /// 共享操作
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Description("共享操作")]
        private static async Task<bool> SaveAsync(Action action)
        {
            #region Get Configuration
            await Task.Run(() =>
            {
                Admin = AppSetting.Get("Shared.Admin");
                Pwd = AppSetting.Get("Shared.Pwd");
                Host = AppSetting.Get("Shared.Host");
                HttpAddress = AppSetting.Get("Shared.Http");
                Port = ":" + AppSetting.Get("Shared.Port");
            });
            #endregion
            using (SharedTool tool = new(Admin, Pwd, Host))
                await Task.Run(action);
            return true;
        }

        /// <summary>
        /// 保存到共享目录
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="ext"></param>
        /// <param name="directory"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [Description("保存到共享目录")]
        private static async Task<string> SaveToShare(byte[] bytes, string ext, string directory, Action<string> action)
        {
            #region Get Configuration
            await Task.Run(() =>
            {
                Admin = AppSetting.Get("Shared.Admin");
                Pwd = AppSetting.Get("Shared.Pwd");
                Host = AppSetting.Get("Shared.Host");
                HttpAddress = AppSetting.Get("Shared.Http");
                Port = ":" + AppSetting.Get("Shared.Port");
            });
            #endregion

            string fileName = Encrypter.Md5Hash(Encoding.ASCII.GetString(bytes));

            var filepath = Path.Combine(directory, ext, fileName + ext);

            int nameCount = 0;
            using (SharedTool tool = new(Admin, Pwd, Host))
            {
                while (File.Exists(filepath))
                {
                    if (bytes.Length == File.ReadAllBytes(filepath).Length)
                        return "http:" + filepath.Replace('\\', '/').Replace(Host, HttpAddress).Replace("/Upload", Port);
                    else
                        filepath = Path.Combine(directory, ext, fileName + "_" + ++nameCount + ext);
                }

                var dir = Path.GetDirectoryName(filepath);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir!);

                await Task.Run(() => action(filepath));
            }
            return "http:" + filepath.Replace('\\', '/').Replace(Host, HttpAddress).Replace("/Upload", Port);
        }
    }
}
