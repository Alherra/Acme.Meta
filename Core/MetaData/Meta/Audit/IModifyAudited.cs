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
    /// 编辑审计信息
    /// </summary>
    [Description("编辑审计信息")]
    public interface IModifyAudited
    {

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [Description("更新时间")]
        [SugarColumn(ColumnDescription = "答复编辑时间", IsNullable = true)]
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 获取或设置 更新者ID
        /// </summary>
        [Description("更新者ID")]
        public long LastModifierId { get; set; }
    }
}
