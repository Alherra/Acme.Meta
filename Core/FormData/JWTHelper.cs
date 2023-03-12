//using JWT;
//using JWT.Algorithms;
//using JWT.Serializers;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Meta
//{
//    public class JWTHelper
//    {

//        /// <summary>
//        /// 加密
//        /// </summary>
//        /// <param name="model"></param>
//        /// <param name="expTime">过期时间</param>
//        /// <returns></returns>
//        public static string JwtEncrypt<T>(T model, DateTime expTime)
//        {
//            //var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
//            //var secondsSinceEpoch = Math.Round((expTime - unixEpoch).TotalSeconds);
//            double exp = (expTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;




//            var payload = ConvertToMap(model);
//            payload.Add("exp", exp);
//            var secret = ConfigHelper.GetSettings("JwtSecret");

//            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//            var token = encoder.Encode(payload, secret);
//            return token;
//        }

//        /// <summary>
//        /// 解密
//        /// </summary>
//        /// <param name="token"></param>
//        /// <returns></returns>
//        public static T JwtDecrypt<T>(string token)
//        {
//            var secret = ConfigHelper.GetSettings("JwtSecret");
//            try
//            {
//                IJsonSerializer serializer = new JsonNetSerializer();
//                IDateTimeProvider provider = new UtcDateTimeProvider();
//                IJwtValidator validator = new JwtValidator(serializer, provider);
//                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

//                var json = decoder.Decode(token, secret, verify: true);
//                return JsonConvert.DeserializeObject<T>(json);
//            }
//            catch (TokenExpiredException)
//            {

//                //throw new Exception("token 过期");
//            }
//            catch (SignatureVerificationException)
//            {
//                //throw new Exception("token 签名无效");
//            }
//            return default(T);
//        }

//        /// <summary>
//        /// 将对象属性转换为key-value对  
//        /// </summary>  
//        /// <param name="o"></param>  
//        /// <returns></returns>  
//        private static Dictionary<String, Object> ConvertToMap(Object o)
//        {
//            Dictionary<String, Object> map = new Dictionary<string, object>();
//            Type t = o.GetType();
//            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
//            foreach (PropertyInfo p in pi)
//            {
//                MethodInfo mi = p.GetGetMethod();
//                if (mi != null && mi.IsPublic)
//                {
//                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
//                }
//            }
//            return map;
//        }
//    }
//}
