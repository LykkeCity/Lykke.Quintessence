using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IBlacklistedAddressRepository
    {
        Task<bool> AddIfNotExistsAsync(
            string address,
            string reason);

        Task<bool> ContainsAsync(
            string address);
        
        Task<(IEnumerable<BlacklistedAddress> Addresses, string ContinuationToken)> GetAllAsync(
            int take,
            string continuationToken);
        
        Task<BlacklistedAddress> TryGetAsync(
            string address);
        
        Task<bool> RemoveIfExistsAsync(
            string address);
    }
}
