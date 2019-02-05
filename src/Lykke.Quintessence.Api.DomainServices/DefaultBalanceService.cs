using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultBalanceService : IBalanceService
    {
        private readonly IBalanceMonitoringTaskRepository _balanceMonitoringTaskRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ILog _log;

        
        public DefaultBalanceService(
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            IBalanceRepository balanceRepository,
            IBlockchainService blockchainService,
            ILogFactory logFactory)
        {
            _balanceMonitoringTaskRepository = balanceMonitoringTaskRepository;
            _balanceRepository = balanceRepository;
            _log = logFactory.CreateLog(this);
        }

        
        /// <inheritdoc />
        public async Task<bool> BeginObservationIfNotObservingAsync(
            string address)
        {
            var observationBegan = await _balanceRepository
                .CreateIfNotExistsAsync(address);

            await _balanceMonitoringTaskRepository.EnqueueAsync
            (
                new BalanceMonitoringTask(address)
            );
            
            if (observationBegan)
            {
                _log.Info
                (
                    $"Wallet [{address}] has been added to balance observation list.",
                    new { address }
                );
            }
            
            return observationBegan;
        }

        /// <inheritdoc />
        public async Task<bool> EndObservationIfObservingAsync(
            string address)
        {
            var observationEnded = await  _balanceRepository
                .DeleteIfExistsAsync(address);
            
            if (observationEnded)
            {
                _log.Info
                (
                    $"Wallet [{address}] has been removed from balance observation list.",
                    new { address }
                );
            }

            return observationEnded;
        }

        /// <inheritdoc />
        public Task<(IReadOnlyCollection<Balance> Balances, string ContinuationToken)> GetTransferableBalancesAsync(
            int take,
            string continuationToken)
        {
            return _balanceRepository
                .GetAllTransferableBalancesAsync(take, continuationToken);
        }
    }
}