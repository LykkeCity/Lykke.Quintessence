using System.Threading.Tasks;
using Lykke.Quintessence.Core.DistributedLock;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IBalanceCheckSchedulerLockRepository
    {
        Task<IDistributedLock> TryLockAsync();
    }
}