using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultBalanceCheckSchedulingService : IBalanceCheckSchedulingService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IBalanceCheckSchedulerLockRepository _balanceCheckSchedulerLockRepository;
        private readonly IBalanceMonitoringTaskRepository _balanceMonitoringTaskRepository;
        private readonly IBlockchainService _blockchainService;
        private readonly ILog _log;
        private readonly BigInteger _maximalBalanceCheckPeriod;

        public DefaultBalanceCheckSchedulingService(
            IBalanceCheckSchedulerLockRepository balanceCheckSchedulerLockRepository,
            IBalanceRepository balanceRepository,
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            IBlockchainService blockchainService,
            ILogFactory logFactory,
            Settings settings)
        {
            _balanceCheckSchedulerLockRepository = balanceCheckSchedulerLockRepository;
            _balanceRepository = balanceRepository;
            _balanceMonitoringTaskRepository = balanceMonitoringTaskRepository;
            _blockchainService = blockchainService;
            _log = logFactory.CreateLog(this);
            _maximalBalanceCheckPeriod = settings.MaximalBalanceCheckPeriod;
        }

        public async Task ScheduleBalanceChecksAsync()
        {
            try
            {
                var @lock = await _balanceCheckSchedulerLockRepository.TryLockAsync();

                if (@lock == null)
                {
                    return;
                }
                
                try
                {
                    var bestTrustedBlockNumber = await _blockchainService.GetBestTrustedBlockNumberAsync();
                    var balanceExpirationBlock = bestTrustedBlockNumber - _maximalBalanceCheckPeriod;
                    var schedulerCounter = 0;

                    string continuationToken = null;

                    do
                    {
                        IEnumerable<Balance> balances;

                        (balances, continuationToken) = await _balanceRepository.GetAllAsync(100, continuationToken);

                        foreach (var balance in balances.Where(x => x.BlockNumber <= balanceExpirationBlock))
                        {
                            await _balanceMonitoringTaskRepository.EnqueueAsync(new BalanceMonitoringTask
                            (
                                address: balance.Address
                            ));

                            schedulerCounter++;
                        }

                        await @lock.RenewIfNecessaryAsync();

                    } while (continuationToken != null);

                    if (schedulerCounter > 0)
                    {
                        _log.Info($"Periodical balance checks scheduled for {schedulerCounter} addresses.");
                    }
                }
                finally
                {
                    await @lock.ReleaseAsync();
                }
            }
            catch (Exception e)
            {
                _log.Warning("Failed to schedule periodical balance checks.", e);
            }
        }

        public class Settings
        {
            public BigInteger MaximalBalanceCheckPeriod { get; set; }
        }
    }
}