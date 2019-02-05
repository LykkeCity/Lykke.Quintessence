using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IAddressService
    {
        Task<string> AddChecksumAsync(
            string address);
        
        Task<AddAddressResult> AddAddressToBlacklistAsync(
            [NotNull] string address,
            [NotNull] string reason);

        Task<AddAddressResult> AddAddressToWhitelistAsync(
            [NotNull] string address,
            BigInteger maxGasAmount);

        Task<(IEnumerable<BlacklistedAddress> BlacklistedAddresses, string ContinuationToken)> GetBlacklistedAddressesAsync(
            int take,
            [CanBeNull] string continuationToken);

        Task<(IEnumerable<WhitelistedAddress> WhitelistedAddresses, string ContinuationToken)> GetWhitelistedAddressesAsync(
            int take,
            [CanBeNull] string continuationToken);

        Task<RemoveAddressResult> RemoveAddressFromBlacklistAsync(
            [NotNull] string address);

        Task<RemoveAddressResult> RemoveAddressFromWhitelistAsync(
            [NotNull] string address);

        [ItemCanBeNull]
        Task<string> TryGetBlacklistingReason(
            [NotNull] string address);

        [ItemCanBeNull]
        Task<BigInteger?> TryGetCustomMaxGasAmountAsync(
            [NotNull] string address);

        Task<AddressValidationResult> ValidateAsync(
            string address,
            bool skipChecksumValidation,
            bool skipWhiteAndBlacklistCheck);
    }
}