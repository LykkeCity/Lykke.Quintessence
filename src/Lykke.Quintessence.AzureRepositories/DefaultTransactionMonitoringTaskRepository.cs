using System;
using AzureStorage.Queue;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultTransactionMonitoringTaskRepository 
        : TaskRepositoryBase<TransactionMonitoringTask>,
          ITransactionMonitoringTaskRepository
    {
        private DefaultTransactionMonitoringTaskRepository(
            IQueueExt queue) : base(queue)
        {
            
        }

        public static ITransactionMonitoringTaskRepository Create(
            IReloadingManager<string> connectionString)
        {
            var queue = AzureQueueExt.Create
            (
                connectionStringManager: connectionString,
                queueName: "transaction-monitoring-tasks",
                maxExecutionTimeout: TimeSpan.FromDays(7)
            );
            
            return new DefaultTransactionMonitoringTaskRepository(queue);
        }
    }
}