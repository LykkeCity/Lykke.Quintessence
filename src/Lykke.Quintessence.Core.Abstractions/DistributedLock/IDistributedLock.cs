using System.Threading.Tasks;

namespace Lykke.Quintessence.Core.DistributedLock
{
    public interface IDistributedLock
    {
        Task ReleaseAsync();

        Task RenewIfNecessaryAsync();
    }
}
