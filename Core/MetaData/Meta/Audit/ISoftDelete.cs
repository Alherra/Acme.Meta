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
    /// Used to standardize soft deleting entities. Soft-delete entities are not actually
    /// deleted, marked as IsDeleted = true in the database, but can not be retrieved
    /// to the application normally.
    /// </summary>
    [Description("SoftDelete Interface")]
    public interface ISoftDelete
    {
        /// <summary>
        /// Used to mark an Entity as 'Deleted'.
        /// </summary>
        [SugarColumn(ColumnDescription = "删除时间", IsNullable = true)]
        [Description("DeletedTime")]
        DateTime? DeletedTime { get; set; }
    }
}
