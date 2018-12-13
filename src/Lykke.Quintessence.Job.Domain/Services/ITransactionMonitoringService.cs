using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface ITransactionMonitoringService
    {
        Task<bool> CheckAndUpdateStateAsync(
            TransactionMonitoringTask task);
        
        Task CompleteMonitoringTaskAsync(
            TransactionMonitoringTask task);
        
        Task<TransactionMonitoringTask> TryGetNextMonitoringTaskAsync();
    }
}
