using System.ComponentModel;

namespace System
{
    /// <summary>
    /// StatusCode for Http requests.
    /// 
    /// Http请求状态码
    /// </summary>
    [Description("StatusCode")]
    public enum StatusCode
    {
        /// <summary>
        /// The request is successfully done.
        /// 
        /// 成功
        /// </summary>
        [Description("Success")]
        Success = 200,

        /// <summary>
        /// An error occurred with bussiness.
        /// 
        /// 业务逻辑异常
        /// </summary>
        [Description("BussinessError")]
        BussinessError = 400,

        /// <summary>
        /// The request needs a legal authorize .
        /// 
        /// 身份验证异常
        /// </summary>
        [Description("UnAuthorize")]
        UnAuthorize = 401,

        /// <summary>
        /// The request is forbid now.
        /// 
        /// 禁止访问
        /// </summary>
        [Description("Forbidden")]
        Forbidden = 403,

        /// <summary>
        /// The request path is not found.
        /// 
        /// 未找到服务
        /// </summary>
        [Description("NotFound")]
        NotFound = 404,

        /// <summary>
        /// Client request too much.
        /// 
        /// 请求过度
        /// </summary>
        [Description("OverFrequency")]
        OverFrequency = 421,

        /// <summary>
        /// An error occurs that some data is not mapped to the target.
        /// 
        /// 数据匹配异常
        /// </summary>
        [Description("DataMapperError")]
        DataMapperError = 440,

        /// <summary>
        /// Server has threw an error.
        /// 
        /// 服务器内部错误
        /// </summary>
        [Description("Server")]
        Server = 460
    }
}
