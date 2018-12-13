using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.QueueConsumers;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Quintessence.QueueConsumers
{
    [UsedImplicitly]
    public class DefaultTransactionMonitoringQueueConsumer : QueueConsumerBase<TransactionMonitoringTask>, ITransactionMonitoringQueueConsumer
    {
        private readonly ILog _log;
        private readonly ITransactionMonitoringService _transactionMonitoringService;
        
        
        public DefaultTransactionMonitoringQueueConsumer(
            ILogFactory logFactory,
            ITransactionMonitoringService transactionMonitoringService,
            int maxDegreeOfParallelism)
        
            : base(TimeSpan.FromSeconds(1), maxDegreeOfParallelism)
        {
            _log = logFactory.CreateLog(this);
            _transactionMonitoringService = transactionMonitoringService;
        }
        
        
        protected override Task<TransactionMonitoringTask> TryGetNextTaskAsync()
        {
            return _transactionMonitoringService.TryGetNextMonitoringTaskAsync();
        }

        protected override async Task ProcessTaskAsync(
            TransactionMonitoringTask task)
        {
            var transactionChecked = await _transactionMonitoringService.CheckAndUpdateStateAsync(task);
            
            if (transactionChecked)
            {
                await _transactionMonitoringService.CompleteMonitoringTaskAsync(task);
            }
        }
        
        public override void Start()
        {
            _log.Info("Starting transaction monitoring...");
            
            base.Start();
            
            _log.Info("Transaction monitoring started.");
        }

        public override void Stop()
        {
            _log.Info("Stopping transaction monitoring...");
            
            base.Stop();
            
            _log.Info("Transaction monitoring stopped.");
        }
    }
}
