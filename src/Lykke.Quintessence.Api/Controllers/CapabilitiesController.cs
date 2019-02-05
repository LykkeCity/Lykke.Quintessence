using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("api/capabilities")]
    public class CapabilitiesController : Controller
    {
        private static readonly CapabilitiesResponse CapabilitiesResponse = new CapabilitiesResponse
        {
            AreManyInputsSupported = false,
            AreManyOutputsSupported = false,
            CanReturnExplorerUrl = false,
            IsAddressMappingRequired = false,
            IsExclusiveWithdrawalsRequired = false,
            IsPublicAddressExtensionRequired = false,
            IsReceiveTransactionRequired = false,
            IsTestingTransfersSupported = false,
            IsTransactionsRebuildingSupported = false
        };

        [HttpGet]  
        public ActionResult<CapabilitiesResponse> Get()
        {
            return CapabilitiesResponse;
        }
    }
}
