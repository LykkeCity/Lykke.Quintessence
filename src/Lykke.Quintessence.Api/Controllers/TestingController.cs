using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("/api/testing")]
    public class TestingController : Controller
    {
        #region Not Implemented Endpoints
        
        [HttpPost("transfers")]
        public ActionResult Transfer(
            [FromBody] TestingTransferRequest request)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpDelete("storage")]
        public ActionResult ClearStorage()
            => StatusCode(StatusCodes.Status501NotImplemented);
        
        #endregion
    }
}
