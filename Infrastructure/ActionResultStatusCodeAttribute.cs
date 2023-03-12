using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Infrastructure
{
    /// <summary>
    /// Attribute annoted on ActionResult constructor and helper method parameters to
    /// indicate that the parameter is used to set the "statusCode" for the ActionResult.
    /// Analyzers match this parameter by type name. This allows users to annotate custom
    /// results \ custom helpers with a user defined attribute without having to expose
    /// this type.
    /// This attribute is intentionally marked Inherited=false since the analyzer does
    /// not walk the inheritance graph.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class ActionResultStatusCodeAttribute : Attribute
    {
    }
}
