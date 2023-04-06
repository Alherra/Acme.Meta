using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Paged result object.
    /// </summary>
    /// <typeparam name="T">Data class type.</typeparam>
    [Description("Paged-Result")]
    public class PagedResult<T>
    {
        /// <summary>
        /// Total count of Items
        /// </summary>
        [Description("List-Count")]
        public virtual long Count
        {
            get;
            set;
        }

        /// <summary>
        /// List of items in current page
        /// </summary>
        [Description("List-Items")]
        public virtual IReadOnlyList<T> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new PagedResultDto`1 object.
        /// </summary>
        /// <param name="count">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        [Description("Paged-Result")]
        public PagedResult(long count, IReadOnlyList<T> items)
        {
            Count = count;
            Items = items;
        }
    }
}
