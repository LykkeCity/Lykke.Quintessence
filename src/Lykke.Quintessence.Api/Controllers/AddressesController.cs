using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Service.BlockchainApi.Contract.Addresses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("/api/addresses")]
    public class AddressesController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressesController(
            IAddressService addressService)
        {
            _addressService = addressService;
        }


        [HttpGet("{address}/validity")]
        public async Task<ActionResult<AddressValidationResponse>> GetAddressValidity(
            string address)
        {
            var validationResult = await _addressService.ValidateAsync
            (
                address: address,
                skipChecksumValidation: false,
                skipWhiteAndBlacklistCheck: false
            );
            
            return new AddressValidationResponse
            {
                IsValid = validationResult is AddressValidationResult.Success
            };
        }

        #region Not Implemented Endpoints
        
        [HttpGet("{address}/balance")]
        public ActionResult GetBalance(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/explorer-url")]
        public ActionResult GetExplorerUrl(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/underlying")]
        public ActionResult GetUnderlyingAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        [HttpGet("{address}/virtual")]
        public ActionResult GetVirtualAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);
        
        #endregion
    }
}
