using System.ComponentModel;

namespace System
{
    /// <summary>
    /// TenantInfo
    /// </summary>
    [Description("TenantInfo")]
    public class TenantInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Tenant name
        /// </summary>
        [Description("TenantName")]
        public virtual string TenantName { get; set; }

        /// <summary>
        /// Manager
        /// </summary>
        [Description("ManagerID")]
        public virtual int ManagerID { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Description("Description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Enable
        /// </summary>
        [Description("Enable")]
        public virtual bool IsEnable { get; set; }
    }
}
