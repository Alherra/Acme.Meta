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
        public virtual int Total
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
        /// <param name="total">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        [Description("Paged-Result")]
        public PagedResult(int total, IReadOnlyList<T> items)
        {
            Total = total;
            Items = items;
        }

    }

    /// <summary>
    /// Paged result object.
    /// </summary>
    public class PagedResult
    {
        /// <summary>
        /// To create an result.
        /// </summary>
        /// <typeparam name="Tr"></typeparam>
        /// <param name="total"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static PagedResult<Tr> Create<Tr>(int total, IReadOnlyList<Tr> items) => new(total, items);

        /// <summary>
        /// To create an result.
        /// </summary>
        /// <typeparam name="Tr"></typeparam>
        /// <param name="total"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static PagedResult<Tr> Create<Tr>(IReadOnlyList<Tr> items, int total) => new(total, items);
    }
}
