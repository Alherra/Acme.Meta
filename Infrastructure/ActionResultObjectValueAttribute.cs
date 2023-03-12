using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Infrastructure
{
    /// <summary>
    /// Attribute annoted on ActionResult constructor, helper method parameters, and
    /// properties to indicate that the parameter or property is used to set the "value"
    /// for ActionResult.
    /// Analyzers match this parameter by type name. This allows users to annotate custom
    /// results \ custom helpers with a user defined attribute without having to expose
    /// this type.
    /// This attribute is intentionally marked Inherited=false since the analyzer does
    /// not walk the inheritance graph.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class ActionResultObjectValueAttribute : Attribute
    {
    }
}
