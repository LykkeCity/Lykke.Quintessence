using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Service.BlockchainApi.Contract;
using Lykke.Service.BlockchainApi.Contract.Balances;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("api/balances")]
    public class BalancesController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IAssetService _assetService;
        private readonly IBalanceService _balanceService;


        public BalancesController(
            IAddressService addressService,
            IAssetService assetService,
            IBalanceService balanceService)
        {
            
            _addressService = addressService;
            _assetService = assetService;
            _balanceService = balanceService;
        }

        
        [HttpPost("{address}/observation")]
        public async Task<ActionResult> AddAddressToObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _balanceService.BeginObservationIfNotObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
        
        [HttpDelete("{address}/observation")]
        public async Task<ActionResult> DeleteAddressFromObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _balanceService.EndObservationIfObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<WalletBalanceContract>>> GetBalanceList(
            PaginationRequest request)
        {
            var (balances, continuation) = await _balanceService.GetTransferableBalancesAsync(request.Take, request.Continuation);

            return new PaginationResponse<WalletBalanceContract>
            {
                Continuation = continuation,
                Items = await balances.SelectItemsAsync(async x => new WalletBalanceContract
                {
                    Address = await _addressService.AddChecksumAsync(x.Address),
                    AssetId = _assetService.Get().Id,
                    Balance = x.Amount.ToString(),
                    Block = (long) x.BlockNumber
                })
            };
        }
    }
}
