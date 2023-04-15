using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 创建审计信息
    /// </summary>
    [Description("创建审计信息")]
    public interface ICreationAudited : ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建者ID
        /// </summary>
        [SugarColumn(ColumnDescription = "创建者ID", IsOnlyIgnoreUpdate = true)]
        [Description("创建者ID")]
        public long CreatorId { get; set; }
    }
}
