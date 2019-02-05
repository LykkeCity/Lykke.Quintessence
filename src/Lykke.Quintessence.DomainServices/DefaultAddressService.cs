using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultAddressService : IAddressService
    {
        private static readonly Regex AddressStringExpression
            = new Regex(@"^0x[0-9a-fA-F]{40}$", RegexOptions.Compiled);

        private readonly IAddChecksumStrategy _addChecksumStrategy;
        private readonly IBlacklistedAddressRepository _blacklistedAddressRepository;
        private readonly IBlockchainService _blockchainService;
        private readonly ILog _log;
        private readonly IValidateChecksumStrategy _validateChecksumStrategy;
        private readonly IWhitelistedAddressRepository _whitelistedAddressRepository;

        
        public DefaultAddressService(
            IAddChecksumStrategy addChecksumStrategy,
            IBlacklistedAddressRepository blacklistedAddressRepository,
            IBlockchainService blockchainService,
            ILogFactory logFactory,
            IValidateChecksumStrategy validateChecksumStrategy,
            IWhitelistedAddressRepository whitelistedAddressRepository)
        {
            _addChecksumStrategy = addChecksumStrategy;
            _blacklistedAddressRepository = blacklistedAddressRepository;
            _blockchainService = blockchainService;
            _log = logFactory.CreateLog(this);
            _validateChecksumStrategy = validateChecksumStrategy;
            _whitelistedAddressRepository = whitelistedAddressRepository;
        }


        public Task<string> AddChecksumAsync(
            string address)
        {
            return _addChecksumStrategy.ExecuteAsync(address);
        }

        public async Task<AddAddressResult> AddAddressToBlacklistAsync(
            string address,
            string reason)
        {
            var addressHasBeenAdded = await _blacklistedAddressRepository.AddIfNotExistsAsync
            (
                address: address,
                reason: reason
            );

            if (addressHasBeenAdded)
            {
                _log.Info
                (
                    $"Address [{address}] has been blacklisted.",
                    new { address }
                );
                
                return AddAddressResult.Success();
            }
            else
            {
                return AddAddressResult.HasAlreadyBeenAdded();
            }
        }

        public async Task<AddAddressResult> AddAddressToWhitelistAsync(
            string address,
            BigInteger maxGasAmount)
        {
            var addressHasBeenAdded = await _whitelistedAddressRepository.AddIfNotExistsAsync
            (
                address,
                maxGasAmount
            );
            
            if (addressHasBeenAdded)
            {
                _log.Info
                (
                    $"Address [{address}] has been whitelisted.",
                    new { address }
                );
                
                return AddAddressResult.Success();
            }
            else
            {
                return AddAddressResult.HasAlreadyBeenAdded();
            }
        }

        public Task<(IReadOnlyCollection<BlacklistedAddress> BlacklistedAddresses, string ContinuationToken)> GetBlacklistedAddressesAsync(
            int take,
            string continuationToken)
        {
            return _blacklistedAddressRepository.GetAllAsync(take, continuationToken);
        }

        public Task<(IReadOnlyCollection<WhitelistedAddress> WhitelistedAddresses, string ContinuationToken)> GetWhitelistedAddressesAsync(
            int take,
            string continuationToken)
        {
            return _whitelistedAddressRepository.GetAllAsync(take, continuationToken);
        }

        public async Task<RemoveAddressResult> RemoveAddressFromBlacklistAsync(
            string address)
        {
            var addressHasBeenRemoved = await _blacklistedAddressRepository.RemoveIfExistsAsync(address);

            if (addressHasBeenRemoved)
            {
                _log.Info
                (
                    $"Address [{address}] has been removed from blacklist.",
                    new { address }
                );
                
                return RemoveAddressResult.Success();
            }
            else
            {
                return RemoveAddressResult.NotFound();
            }
        }

        public async Task<RemoveAddressResult> RemoveAddressFromWhitelistAsync(
            string address)
        {
            var addressHasBeenRemoved = await _whitelistedAddressRepository.RemoveIfExistsAsync(address);

            if (addressHasBeenRemoved)
            {
                _log.Info
                (
                    $"Address [{address}] has been removed from whitelist.",
                    new { address }
                );
                
                return RemoveAddressResult.Success();
            }
            else
            {
                return RemoveAddressResult.NotFound();
            }
        }

        public async Task<string> TryGetBlacklistingReason(
            string address)
        {
            return (await _blacklistedAddressRepository.TryGetAsync(address))?
                .BlacklistingReason;
        }

        public async Task<BigInteger?> TryGetCustomMaxGasAmountAsync(
            string address)
        {
            return (await _whitelistedAddressRepository.TryGetAsync(address))?
                .MaxGasAmount;
        }

        public async Task<AddressValidationResult> ValidateAsync(
            string address,
            bool skipChecksumValidation,
            bool skipWhiteAndBlacklistCheck)
        {
            if (!AddressStringExpression.IsMatch(address))
            {
                return AddressValidationResult.AddressIsInvalid(AddressValidationError.FormatIsInvalid);
            }

            if (skipChecksumValidation || !await _validateChecksumStrategy.ExecuteAsync(address))
            {
                return AddressValidationResult.AddressIsInvalid(AddressValidationError.ChecksumIsInvalid);
            }

            if (skipWhiteAndBlacklistCheck)
            {
                address = address.ToLowerInvariant();
            
                if (await _blockchainService.IsContractAsync(address) && 
                   !await _whitelistedAddressRepository.ContainsAsync(address) &&
                    await _blacklistedAddressRepository.ContainsAsync(address))
                {
                    return AddressValidationResult.AddressIsInvalid(AddressValidationError.AddressIsBlacklisted);
                }
            }
            
            return AddressValidationResult.AddressIsValid();
        }
    }
}