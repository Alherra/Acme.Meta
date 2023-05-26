using System.ComponentModel;

namespace System
{
    /// <summary>
    /// UserInfo
    /// </summary>
    [Description("UserInfo")]
    public class UserInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public int Id { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [Description("UserName")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [Description("E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [Description("PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Enable
        /// </summary>
        [Description("Enable")]
        public virtual bool IsEnable { get; set; }

        /// <summary>
        /// Id of the related tenant.
        /// </summary>
        [Description("Id of the related tenant.")]
        public int TenantId { get; set; }
    }
}
