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
    public sealed class DefaultBalanceMonitoringQueueConsumer : QueueConsumerBase<BalanceMonitoringTask>, IBalanceMonitoringQueueConsumer
    {
        private readonly IBalanceMonitoringService _balanceMonitoringService;
        private readonly ILog _log;
        
        
        public DefaultBalanceMonitoringQueueConsumer(
            IBalanceMonitoringService balanceMonitoringService,
            ILogFactory logFactory,
            int maxDegreeOfParallelism)
            
            : base(TimeSpan.FromSeconds(1), maxDegreeOfParallelism)
        {
            _balanceMonitoringService = balanceMonitoringService;
            _log = logFactory.CreateLog(this);
        }

        protected override Task<BalanceMonitoringTask> TryGetNextTaskAsync()
        {
            return _balanceMonitoringService.TryGetNextMonitoringTaskAsync();
        }

        protected override async Task ProcessTaskAsync(
            BalanceMonitoringTask task)
        {
            var balanceChecked = await _balanceMonitoringService.CheckAndUpdateBalanceAsync(task);

            if (balanceChecked)
            {
                await _balanceMonitoringService.CompleteMonitoringTaskAsync(task);
            }
        }

        public override void Start()
        {
            _log.Info("Starting balances monitoring...");
            
            base.Start();
            
            _log.Info("Balances monitoring started.");
        }

        public override void Stop()
        {
            _log.Info("Stopping balances monitoring...");
            
            base.Stop();
            
            _log.Info("Balances monitoring stopped.");
        }
    }
}
