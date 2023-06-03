using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface ISugarDB
    {
        /// <summary>
        /// Client
        /// </summary>
        [Description("Client")]
        SqlSugarClient Client { get; }

        /// <summary>
        /// Scope
        /// </summary>
        [Description("Scope")]
        SqlSugarScope Scope { get; }
    }
}
