using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// Paged by input object.
    /// </summary>
    [DisplayName("PagedByObject")]
    [Description("PagedByObject")]
    public abstract class PagedInput
    {
        /// <summary>
        /// The index number of the page.
        /// </summary>
        [DisplayName("Page-Number")]
        [Description("Page-Number")]
        public virtual int PageNum { get; set; }

        /// <summary>
        /// The size number of the page.
        /// </summary>
        [DisplayName("Page-Size")]
        [Description("Page-Size")]
        public virtual int PageSize { get; set; }
    }
}
