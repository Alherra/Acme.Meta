
using System.ComponentModel;

namespace Meta
{
    /// <summary>
    /// An attribute that create injection for a type class.
    /// 
    /// 依赖注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Description("依赖注入特性")]
    public class AutoInjectAttribute : Attribute
    {
        /// <summary>
        /// Define the type of the injection.
        /// 
        /// 依赖注入
        /// </summary>
        /// <param name="injectType">注入类型</param>
        [Description("依赖注入")]
        public AutoInjectAttribute(InjectType injectType = InjectType.Scope)
        {
            InjectType = injectType;
        }

        /// <summary>
        /// The type of the injection.
        /// 
        /// 注入类型
        /// </summary>
        [Description("注入类型")]
        public InjectType InjectType { get; set; }
    }

    /// <summary>
    /// The types for the injection.
    /// 
    /// 注入类型
    /// </summary>
    [Description("注入类型")]
    public enum InjectType
    {
        /// <summary>
        /// Scope
        /// </summary>
        [Description("Scope")]
        Scope,

        /// <summary>
        /// Single
        /// </summary>
        [Description("Single")]
        Single,

        /// <summary>
        /// Transient
        /// </summary>
        [Description("Transient")]
        Transient
    }
}
