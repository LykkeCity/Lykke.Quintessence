using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IBalanceMonitoringService
    {
        Task<BalanceMonitoringTask> TryGetNextMonitoringTaskAsync();

        Task<bool> CheckAndUpdateBalanceAsync(
            BalanceMonitoringTask task);

        Task CompleteMonitoringTaskAsync(
            BalanceMonitoringTask task);
    }
}
