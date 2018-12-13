using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultBalanceMonitoringService : IBalanceMonitoringService
    {
        private readonly IBalanceMonitoringTaskRepository _balanceMonitoringTaskRepository;
        private readonly IBlockchainService _blockchainService;
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;
        private readonly IBalanceRepository _balanceRepository;


        public DefaultBalanceMonitoringService(
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            IBalanceRepository balanceRepository,
            IBlockchainService blockchainService,
            IChaosKitty chaosKitty,
            ILogFactory logFactory)
        {
            _balanceMonitoringTaskRepository = balanceMonitoringTaskRepository;
            _blockchainService = blockchainService;
            _chaosKitty = chaosKitty;
            _log = logFactory.CreateLog(this);
            _balanceRepository = balanceRepository;
        }


        public async Task<bool> CheckAndUpdateBalanceAsync(
            BalanceMonitoringTask task)
        {
            var address = task.Address;
            
            try
            {
                var balance = await _balanceRepository.TryGetAsync(address);

                if (balance != null)
                {
                    var bestBlockNumber = await _blockchainService.GetBestTrustedBlockNumberAsync();
                    var amount = await _blockchainService.GetBalanceAsync(address, bestBlockNumber);
                
                    balance.OnUpdated
                    (
                        amount: amount,
                        blockNumber: bestBlockNumber
                    );
                    
                    _chaosKitty.Meow(address);    
                    
                    await _balanceRepository.UpdateSafelyAsync(balance);
                    
                    _log.Info
                    (
                        $"Account [{address}] balance updated to [{amount}] at block [{bestBlockNumber}].",
                        new { address }
                    );
                }
                else
                {
                    _log.Info
                    (
                        $"Account [{address}] balance is not observable.",
                        new { address }
                    );
                }

                return true;
            }
            catch (Exception e)
            {
                _log.Warning
                (
                    $"Failed to check balance of account [{task.Address}].",
                    e,
                    new { address }
                );

                return false;
            }
        }

        public Task CompleteMonitoringTaskAsync(
            BalanceMonitoringTask task)
        {
            return _balanceMonitoringTaskRepository.CompleteAsync(task);
        }

        public Task<BalanceMonitoringTask> TryGetNextMonitoringTaskAsync()
        {
            return _balanceMonitoringTaskRepository.TryGetAsync
            (
                visibilityTimeout: TimeSpan.FromMinutes(1)
            );
        }
    }
}
