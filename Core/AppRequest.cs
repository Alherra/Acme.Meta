using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Tools
{
    /// <summary>
    /// Http请求
    /// </summary>
    [Description("Http请求")]
    public class AppRequest
    {
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        [Description("Post")]
        public static string Post<T>(string url, T t)
        {
            var data = JsonConvert.SerializeObject(t);
            return PostDataViaHttpWebRequest(url, null, null, data);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        [Description("Get请求")]
        public static string GetRequest(string url, Dictionary<string, string> headers = null!, int Timeout = 50000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;
            if (headers != null)
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgent"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        [Description("创建POST方式的HTTP请求")]
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest? request;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request!.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
            }
            string[] values = request.Headers.GetValues("Content-Type")!;
            return (request.GetResponse() as HttpWebResponse)!;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        [Description("获取请求的数据")]
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using Stream s = webresponse.GetResponseStream();
            StreamReader reader = new(s, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="headers"></param>
        /// <param name="urlParas"></param>
        /// <param name="requestBody"></param>
        /// <returns>接口返回值</returns>
        [Description("Post请求")]
        private static string PostDataViaHttpWebRequest(string baseUrl,
            IReadOnlyDictionary<string, string> headers,
            IReadOnlyDictionary<string, string> urlParas,
            string requestBody = null)
        {
            var resuleJson = string.Empty;
            try
            {
                var apiUrl = baseUrl;
                if (urlParas != null)
                    foreach (var p in urlParas)
                    {
                        if (apiUrl.IndexOf("{" + p.Key + "}") > -1)
                        {
                            apiUrl = apiUrl.Replace("{" + p.Key + "}", p.Value);
                        }
                        else
                        {
                            apiUrl += string.Format("{0}{1}={2}", apiUrl.Contains('?') ? "&" : "?", p.Key, p.Value);
                        }
                    }
                //urlParas.ForEach(p =>
                //{
                //    if (apiUrl.IndexOf("{" + p.Key + "}") > -1)
                //    {
                //        apiUrl = apiUrl.Replace("{" + p.Key + "}", p.Value);
                //    }
                //    else
                //    {
                //        apiUrl += string.Format("{0}{1}={2}", apiUrl.Contains("?") ? "&" : "?", p.Key, p.Value);
                //    }
                //});

                var req = (HttpWebRequest)WebRequest.Create(apiUrl);
                req.Method = "POST";
                req.ContentType = "application/json"; //Defalt

                if (!string.IsNullOrEmpty(requestBody))
                {
                    using var postStream = new StreamWriter(req.GetRequestStream());
                    postStream.Write(requestBody);
                }

                if (headers != null)
                {
                    if (headers.Keys.Any(p => p.ToLower() == "content-type"))
                        req.ContentType = headers.SingleOrDefault(p => p.Key.ToLower() == "content-type").Value;
                    if (headers.Keys.Any(p => p.ToLower() == "accept"))
                        req.Accept = headers.SingleOrDefault(p => p.Key.ToLower() == "accept").Value;
                }

                var response = (HttpWebResponse)req.GetResponse();

                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new(stream, Encoding.GetEncoding("UTF-8"));
                resuleJson = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return resuleJson;// JsonConvert.DeserializeObject<T>(resuleJson);
        }
    }
}
