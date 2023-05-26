using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace System
{
    /// <summary>
    /// ApiControllerBase
    /// </summary>
    [ApiController]
    [Authorization]
    [Route("api/[controller]/[action]")]
    public abstract class WebAPI : Meta.MetaController { }
}
