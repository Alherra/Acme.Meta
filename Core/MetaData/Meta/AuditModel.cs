using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Audit base
    /// </summary>
    public abstract class AuditModel : Meta<int>, IMultiTenant, ICreationAudited, IModifyAudited, IDeletionAudited
    {
        [SugarColumn(ColumnDescription = "CreatorId")]
        public int CreatorId { get; set; }

        [SugarColumn(ColumnDescription = "CreatedTime")]
        public DateTime CreatedTime { get; set; }

        [SugarColumn(ColumnDescription = "LastModificationTime", IsNullable = true)]
        public DateTime? LastModificationTime { get; set; }

        [SugarColumn(ColumnDescription = "LastModifierId", IsNullable = true)]
        public int LastModifierId { get; set; }

        [SugarColumn(ColumnDescription = "DeleterId", IsNullable = true)]
        public int? DeleterId { get; set; }

        [SugarColumn(ColumnDescription = "DeletedTime", IsNullable = true)]
        public DateTime? DeletedTime { get; set; }

        [SugarColumn(ColumnDescription = "TenantId", IsNullable = true)]
        public int TenantId { get; set; }
    }
}
