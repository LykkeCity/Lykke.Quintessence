using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IBlockIndexationLockRepository
    {
        Task DeleteIfExistsAsync(
            BigInteger blockNumber);
        
        Task<IEnumerable<BlockIndexationLock>> GetAsync();
        
        Task InsertOrReplaceAsync(
            BigInteger blockNumber);
    }
}
