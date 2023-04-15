using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;

namespace System
{
    public static class TencentSMS
    {
        /// <summary>
        /// 发短信
        /// </summary>
        /// <param name="message"></param>
        /// <param name="phoneNumber">手机号，不含+86</param>
        /// <param name="template"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async static Task Send(string message, ulong phoneNumber, SendTemplate template, ushort state = 86)
        {
            string SecretId = string.Empty,
            SecretKey = string.Empty,
            SmsSdkAppId = string.Empty,
            SignName = string.Empty;
            await Task.Run(() =>
            {
                SecretId = AppSetting.Get("TencentSms.SecretId");
                SecretKey = AppSetting.Get("TencentSms.SecretKey");
                SmsSdkAppId = AppSetting.Get("TencentSms.SmsSdkAppId");
                SignName = AppSetting.Get("TencentSms.SignName");
            });
            Credential cred = new()
            {
                SecretId = SecretId,
                SecretKey = SecretKey
            };

            try
            {
                ClientProfile clientProfile = new();
                clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;

                /* 非必要步骤
                     * 实例化一个客户端配置对象，可以指定超时时间等配置 */
                HttpProfile httpProfile = new();

                /* SDK默认使用POST方法。
                 * 如果你一定要使用GET方法，可以在这里设置。GET方法无法处理一些较大的请求 */
                // httpProfile.ReqMethod = "GET";
                /* SDK有默认的超时时间，非必要请不要进行调整
                 * 如有需要请在代码中查阅以获取最新的默认值 */
                httpProfile.Timeout = 60; // 请求连接超时时间，单位为秒(默认60秒)

                /* SDK会自动指定域名。通常是不需要特地指定域名的，但是如果你访问的是金融区的服务
                 * 则必须手动指定域名，例如sms的上海金融区域名： sms.ap-shanghai-fsi.tencentcloudapi.com */
                httpProfile.Endpoint = "sms.tencentcloudapi.com";

                // 代理服务器，当你的环境下有代理服务器时设定
                // httpProfile.WebProxy = Environment.GetEnvironmentVariable("HTTPS_PROXY");

                clientProfile.HttpProfile = httpProfile;

                /* 实例化要请求产品(以sms为例)的client对象
                 * 第二个参数是地域信息，可以直接填写字符串ap-guangzhou，或者引用预设的常量 */
                SmsClient client = new(cred, "ap-guangzhou", clientProfile);

                /* 实例化一个请求对象，根据调用的接口和实际情况，可以进一步设置请求参数
                    * 你可以直接查询SDK源码确定SendSmsRequest有哪些属性可以设置
                    * 属性可能是基本类型，也可能引用了另一个数据结构
                    * 推荐使用IDE进行开发，可以方便的跳转查阅各个接口和数据结构的文档说明 */
                SendSmsRequest req = new();

                /* 基本类型的设置:
                 * SDK采用的是指针风格指定参数，即使对于基本类型你也需要用指针来对参数赋值。
                 * SDK提供对基本类型的指针引用封装函数
                 * 帮助链接：
                 * 短信控制台: https://console.cloud.tencent.com/smsv2
                 * sms helper: https://cloud.tencent.com/document/product/382/3773 */
                req.SmsSdkAppId = SmsSdkAppId;

                /* 短信签名内容: 使用 UTF-8 编码，必须填写已审核通过的签名，签名信息可登录 [短信控制台] 查看 */
                req.SignName = SignName;

                /* 短信码号扩展号: 默认未开通，如需开通请联系 [sms helper] */
                req.ExtendCode = "";

                /* 国际/港澳台短信 senderid: 国内短信填空，默认未开通，如需开通请联系 [sms helper] */
                req.SenderId = "";

                /* 用户的 session 内容: 可以携带用户侧 ID 等上下文信息，server 会原样返回 */
                req.SessionContext = "";

                /* 下发手机号码，采用 E.164 标准，+[国家或地区码][手机号]
                 * 示例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号，最多不要超过200个手机号*/
                req.PhoneNumberSet = new string[] { $"+{state}{phoneNumber}" };

                /* 模板 ID: 必须填写已审核通过的模板 ID。模板ID可登录 [短信控制台] 查看 */
                req.TemplateId = $"{(long)template}";

                /* 模板参数: 若无模板参数，则设置为空*/
                req.TemplateParamSet = new string[] { message };

                // 通过client对象调用DescribeInstances方法发起请求。注意请求方法名与请求对象是对应的
                // 返回的resp是一个DescribeInstancesResponse类的实例，与请求对象对应
                SendSmsResponse resp = await client.SendSms(req);

                Console.WriteLine(AbstractModel.ToJsonString(resp));

                if (resp.SendStatusSet != null && resp.SendStatusSet[0] != null)
                {
                    SendStatus status = resp.SendStatusSet[0];
                    if (status.Code?.ToLower() == "ok")
                    {
                        //// 发送结果
                        //MessageReturnDto result = new MessageReturnDto()
                        //{
                        //    Code = status.Code,
                        //    Message = status.Message,
                        //    PhoneNumber = status.PhoneNumber,
                        //    CheckCode = templateParam,
                        //    Action = action,
                        //    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        //};

                        //// 存入短信历史记录
                        //List<MessageReturnDto> messageList = await GetSmsMessageList() ?? new List<MessageReturnDto>();
                        //messageList = messageList.Where(u => !u.CheckCode.IsNullOrEmpty() && DateTime.TryParse(u.Date, out DateTime dt) && (DateTime.Now - dt).TotalSeconds <= 60 * 10).ToList();
                        //messageList.Add(result);
                        //string writeJson = JsonConvert.SerializeObject(new { SmsMessage = messageList }, Formatting.Indented);
                        //await File.WriteAllTextAsync("SmsMessage.json", writeJson);

                        return;
                    }
                    else
                    {
                        throw new Exception(status.Code);
                    }
                }
                throw new Exception("发送短信失败！SendSmsResponse为空");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                throw new Exception(ex.ToString());
            }
        }
    }

    /// <summary>
    /// 模板ID
    /// </summary>
    public enum SendTemplate
    {
        ChangePassword = 1290053,
        ChangePasswordSuccess = 1290058,
        ResetPassword = 1281027,
        ResetPasswordSuccess = 1288979,
        Register = 1288972,
        RegisterSuccess = 1288982,
        ChangePhoneNumber = 1288974,
        ChangePhoneNumberSuccess = 1288981,
        ServerError = 1347234
    }
}
