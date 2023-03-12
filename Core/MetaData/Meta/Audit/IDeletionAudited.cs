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
    /// 软删除审计功能
    /// </summary>
    [Description("软删除审计功能")]
    public interface IDeletionAudited : ISoftDelete
    {
        /// <summary>
        /// Id of the deleter user.
        /// </summary>
        [SugarColumn(ColumnDescription = "删除操作人ID", IsNullable = true)]
        [Description("DeleterUserId")]
        long? DeleterId { get; set; }
    }
}
