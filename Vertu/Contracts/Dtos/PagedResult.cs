using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Description("分页查询结果")]
    public class PagedResult<T>
    {
        /// <summary>
        /// Total count of Items
        /// </summary>
        [Description("总数量")]
        public long TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// List of items in current page
        /// </summary>
        [Description("分页列表")]
        public IReadOnlyList<T> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new Volo.Abp.Application.Dtos.PagedResultDto`1 object.
        /// </summary>
        [Description("分页查询结果")]
        public PagedResult()
        {
        }

        /// <summary>
        /// Creates a new Volo.Abp.Application.Dtos.PagedResultDto`1 object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        [Description("分页查询结果")]
        public PagedResult(long totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
