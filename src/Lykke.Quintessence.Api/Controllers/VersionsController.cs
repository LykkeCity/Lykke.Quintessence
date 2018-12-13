using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("api/versions")]
    public class VersionsController : Controller
    {
        #region Not Implemented Endpoints
        
        [HttpGet]
        public ActionResult Get()
            => StatusCode(StatusCodes.Status501NotImplemented);
        
        #endregion
    }
}
