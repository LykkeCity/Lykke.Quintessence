using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IWhitelistedAddressRepository
    {
        Task<bool> AddIfNotExistsAsync(
            string address,
            BigInteger maxGasAmount);

        Task<bool> ContainsAsync(
            string address);
        
        Task<(IReadOnlyCollection<WhitelistedAddress> Addresses, string ContinuationToken)> GetAllAsync(
            int take,
            string continuationToken);
        
        Task<bool> RemoveIfExistsAsync(
            string address);

        Task<WhitelistedAddress> TryGetAsync(
            string address);
    }
}
