using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Account
{
    /// <summary>
    /// 控制方法接口
    /// </summary>
    [Description("控制方法接口")]
    public class ControllerOptions
    {
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签")]
        public string Tag { get; set; } = String.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// 用户权限
        /// </summary>
        [Description("用户权限")]
        public bool UserOption  { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        [Description("接口")]
        public List<ActionOption> Actions { get; set; } = new List<ActionOption>();
    }

    /// <summary>
    /// 控制方法
    /// </summary>
    [Description("控制方法")]
    public class ActionOption
    {
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签")]
        public string ActionName { get; set; } = String.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// 用户权限
        /// </summary>
        [Description("用户权限")]
        public bool UserOption { get; set; }
    }
}
