using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Meta
{
    /// <summary>
    /// ApiControllerBase
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class ApiMetaController : MetaController
    {
    }
}
