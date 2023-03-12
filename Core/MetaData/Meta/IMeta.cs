using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// 元数据接口
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    [Description("元数据接口")]
    internal interface IMeta<TKey>
    {
        TKey Id { get; set; }
    }

    /// <summary>
    /// 元数据接口
    /// </summary>
    [Description("元数据接口")]
    public interface IMeta
    {
    }
}
