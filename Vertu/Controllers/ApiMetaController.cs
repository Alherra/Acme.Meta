using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace System
{
    /// <summary>
    /// ApiControllerBase
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class WebAPI : Meta.MetaController { }
}
