using System.Threading.Tasks;
using Lykke.Quintessence.Core.DistributedLock;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IBlockchainIndexationStateRepository
    {
        Task<BlockchainIndexationState> GetOrCreateAsync();

        Task UpdateAsync(
            BlockchainIndexationState state);

        Task<IDistributedLock> WaitLockAsync();
    }
}
