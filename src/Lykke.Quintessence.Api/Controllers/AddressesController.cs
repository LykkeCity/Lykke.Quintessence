using System;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Service.BlockchainApi.Contract;
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
        
        [HttpPost("blacklist/{address}")]
        public async Task<IActionResult> AddAddressToBlacklist(
            BlacklistAddressRequest request)
        {
            var result = await _addressService.AddAddressToBlacklistAsync
            (
                address: request.Address.ToLowerInvariant(),
                reason: request.BlacklistingReason
            );

            switch (result)
            {
                case AddAddressResult.SuccessResult _:
                    return Ok();
                
                case AddAddressResult.HasAlreadyBeenAddedError _:
                    return Conflict();
                
                default:
                    throw new NotSupportedException(
                        $"{nameof(_addressService.AddAddressToBlacklistAsync)} returned unsupported result.");
            }
        }
        
        [HttpPost("whitelist/{address}")]
        public async Task<IActionResult> AddAddressToWhitelist(
            WhitelistAddressRequest request)
        {
            var result = await _addressService.AddAddressToWhitelistAsync
            (
                address: request.Address.ToLowerInvariant(),
                maxGasAmount: BigInteger.Parse(request.MaxGasAmount)
            );
            
            switch (result)
            {
                case AddAddressResult.SuccessResult _:
                    return Ok();
                
                case AddAddressResult.HasAlreadyBeenAddedError _:
                    return Conflict();
                
                default:
                    throw new NotSupportedException(
                        $"{nameof(_addressService.AddAddressToWhitelistAsync)} returned unsupported result.");
            }
        }

        [HttpGet("blacklist")]
        public async Task<ActionResult<PaginationResponse<BlacklistedAddressResponse>>> GetBlacklistedAddresses(
            PaginationRequest request)
        {
            var (addresses, continuationToken) = await _addressService.GetBlacklistedAddressesAsync(request.Take, request.Continuation);
            
            var result = new PaginationResponse<BlacklistedAddressResponse>()
            {
                Continuation = continuationToken,
                Items = addresses.Select(async x => new BlacklistedAddressResponse
                {
                    Address = await _addressService.AddChecksumAsync(x.Address),
                    BlacklistingReason = x.BlacklistingReason
                })
                .Select(x => x.Result)
                .ToImmutableArray()
            };
            
            return Ok(result);
        }
        
        [HttpGet("blacklist/{address}/reason")]
        public async Task<ActionResult<BlacklistingReasonResponse>> GetBlacklistingReason(
            AddressRequest request)
        {
            var reason = await _addressService.TryGetBlacklistingReason(request.Address.ToLowerInvariant());

            if (reason != null)
            {
                return new BlacklistingReasonResponse
                {
                    Reason = reason
                };
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpGet("whitelist")]
        public async Task<ActionResult<PaginationResponse<WhitelistedAddressResponse>>> GetWhitelistedAddresses(
            PaginationRequest request)
        {
            var (addresses, continuationToken) = await _addressService.GetWhitelistedAddressesAsync(request.Take, request.Continuation);
            
            var result = new PaginationResponse<WhitelistedAddressResponse>()
            {
                Continuation = continuationToken,
                Items = addresses.Select(async x => new WhitelistedAddressResponse
                {
                    Address = await _addressService.AddChecksumAsync(x.Address),
                    MaxGasAmount = x.MaxGasAmount.ToString()
                })
                .Select(x => x.Result)
                .ToImmutableArray()
            };
            
            return Ok(result);
        }
        
        [HttpDelete("blacklist/{address}")]
        public async Task<IActionResult> RemoveAddressFromBlacklist(
            AddressRequest request)
        {
            var result = await _addressService.RemoveAddressFromBlacklistAsync(request.Address.ToLowerInvariant());
            
            switch (result)
            {
                case RemoveAddressResult.SuccessResult _:
                    return Ok();
                
                case RemoveAddressResult.NotFoundError _:
                    return NoContent();
                
                default:
                    throw new NotSupportedException(
                        $"{nameof(_addressService.RemoveAddressFromBlacklistAsync)} returned unsupported result.");
            }
        }
        
        [HttpDelete("whitelist/{address}")]
        public async Task<IActionResult> RemoveAddressFromWhitelist(
            AddressRequest request)
        {
            var result = await _addressService.RemoveAddressFromWhitelistAsync(request.Address.ToLowerInvariant());
            
            switch (result)
            {
                case RemoveAddressResult.SuccessResult _:
                    return Ok();
                
                case RemoveAddressResult.NotFoundError _:
                    return NoContent();
                
                default:
                    throw new NotSupportedException(
                        $"{nameof(_addressService.RemoveAddressFromWhitelistAsync)} returned unsupported result.");
            }
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
