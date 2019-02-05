using System;
using AzureStorage.Queue;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBalanceMonitoringTaskRepository 
        : TaskRepositoryBase<BalanceMonitoringTask>,
          IBalanceMonitoringTaskRepository
    {
        private DefaultBalanceMonitoringTaskRepository(
            IQueueExt queue) : base(queue)
        {
            
        }
        
        public static IBalanceMonitoringTaskRepository Create(
            IReloadingManager<string> connectionString)
        {
            var queue = AzureQueueExt.Create
            (
                connectionStringManager: connectionString,
                queueName: "balance-observation-tasks",
                maxExecutionTimeout: TimeSpan.FromDays(7)
            );
            
            return new DefaultBalanceMonitoringTaskRepository(queue);
        }
    }
}