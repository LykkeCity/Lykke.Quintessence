using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Service.BlockchainApi.Contract.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("/api/transactions/history")]
    public class TransactionsHistoryController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IAssetService _assetService;
        private readonly ITransactionHistoryService _transactionHistoryService;

        
        public TransactionsHistoryController(
            IAddressService addressService,
            IAssetService assetService,
            ITransactionHistoryService transactionHistoryService)
        {
            _addressService = addressService;
            _assetService = assetService;
            _transactionHistoryService = transactionHistoryService;
        }
        
        
        [HttpPost("to/{address}/observation")]
        public async Task<IActionResult> AddAddressToIncomingHistoryObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _transactionHistoryService.BeginIncomingHistoryObservationIfNotObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
        
        [HttpPost("from/{address}/observation")]
        public async Task<IActionResult> AddAddressToOutgoingHistoryObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _transactionHistoryService.BeginOutgoingHistoryObservationIfNotObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
        
        [HttpDelete("to/{address}/observation")]
        public async Task<IActionResult> DeleteAddressFromIncomingHistoryObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _transactionHistoryService.EndIncomingHistoryObservationIfObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpDelete("from/{address}/observation")]
        public async Task<IActionResult> DeleteAddressToOutgoingHistoryObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _transactionHistoryService.EndOutgoingHistoryObservationIfObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpGet("to/{address}")]
        public async Task<ActionResult<IEnumerable<HistoricalTransactionContract>>> GetIncomingHistory(
            TransactionHistoryRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            var transactions = await _transactionHistoryService.GetIncomingHistoryAsync
            (
                address,
                request.Take,
                request.AfterHash
            );
            
            return Ok(await transactions.SelectAsync(ConvertToHistoricalTransactionContractAsync));
        }

        [HttpGet("from/{address}")]
        public async Task<ActionResult<IEnumerable<HistoricalTransactionContract>>> GetOutgoingHistory(
            TransactionHistoryRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            var transactions = await _transactionHistoryService.GetOutgoingHistoryAsync
            (
                address,
                request.Take,
                request.AfterHash
            );
            
            return Ok(await transactions.SelectAsync(ConvertToHistoricalTransactionContractAsync));
        }

        private async Task<HistoricalTransactionContract> ConvertToHistoricalTransactionContractAsync(
            TransactionReceipt transactionReceipt)
        {
            return new HistoricalTransactionContract
            {
                Amount = transactionReceipt.Amount.ToString(),
                AssetId = _assetService.Get().Id,
                FromAddress = await _addressService.AddChecksumAsync(transactionReceipt.From),
                Hash = transactionReceipt.Hash,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) transactionReceipt.Timestamp).UtcDateTime,
                ToAddress = await _addressService.AddChecksumAsync(transactionReceipt.To)
            };
        }
    }
}
