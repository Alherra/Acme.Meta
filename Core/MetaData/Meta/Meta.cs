using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 元数据基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Description("元数据基类")]
    public abstract class Meta<TKey> : IMeta<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="Meta{TKey}"/>类型的新实例
        /// </summary>
        [Description("初始化主键ID")]
        protected Meta()
        {
            if (typeof(TKey) == typeof(Guid))
                GetType().GetProperty("Id")?.SetValue(this, Guid.NewGuid());
        }

        private TKey? _key;

        /// <summary>
        /// Key ID
        /// </summary>
        [Description("主键ID")]
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public virtual TKey Id 
        { 
            get => _key ?? default!; 
            set => _key = value; 
        }
    }
}
