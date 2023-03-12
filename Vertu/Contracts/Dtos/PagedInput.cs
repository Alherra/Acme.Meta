using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 分页参数
    /// </summary>
    [DisplayName("分页参数")]
    [Description("分页参数")]
    public abstract class PagedInput
    {
        /// <summary>
        /// 跳转页码
        /// </summary>
        [DisplayName("跳转页码")]
        [Description("跳转页码")]
        public virtual int SkipCount { get; set; }

        /// <summary>
        /// 页条数
        /// </summary>
        [DisplayName("页条数")]
        [Description("页条数")]
        public virtual int PageSize { get; set; }
    }
}
