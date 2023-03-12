using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 代理参数
    /// </summary>
    [Description("代理参数")]
    public class TInPut<T>
    {
        /// <summary>
        /// 代理参数值
        /// </summary>
        [Description("代理参数值")]
        public T TValue { get; set; } = default!;
    }
}
