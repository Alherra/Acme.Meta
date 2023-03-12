﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta
{
    /// <summary>
    /// Related to tenant.
    /// </summary>
    [Description("MultiTenant Interface")]
    public interface IMultiTenant
    {
        /// <summary>
        /// Id of the related tenant.
        /// </summary>
        [Description("Id of the related tenant.")]
        long TenantId { get; }
    }
}
