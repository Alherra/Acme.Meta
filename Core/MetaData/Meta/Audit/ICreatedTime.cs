using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Meta.Audit
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    public interface ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true)]
        [Description("创建时间")]
        DateTime CreatedTime { get; set; }
    }
}
