using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultBlockchainIndexingService : IBlockchainIndexingService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IBalanceMonitoringTaskRepository _balanceMonitoringTaskRepository;
        private readonly IBlockchainService _blockchainService;
        private readonly TimeSpan _blockLockDuration;
        private readonly IBlockIndexationLockRepository _blockLockRepository;
        private readonly IChaosKitty _chaosKitty;
        private readonly IIndexBlockStrategy _indexBlockStrategy;
        private readonly ILog _log;
        private readonly BigInteger _minBlockNumberToIndex;
        private readonly IBlockchainIndexationStateRepository _stateRepository;
        private readonly ITransactionReceiptRepository _transactionReceiptRepository;

        
        public DefaultBlockchainIndexingService(
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            IBalanceRepository balanceRepository,
            IBlockchainIndexationStateRepository stateRepository,
            IBlockchainService blockchainServiceService,
            IBlockIndexationLockRepository blockLockRepository,
            IChaosKitty chaosKitty,
            IIndexBlockStrategy indexBlockStrategy,
            ILogFactory logFactory,
            Settings settings,
            ITransactionReceiptRepository transactionReceiptRepository)
        {
            _balanceRepository = balanceRepository;
            _balanceMonitoringTaskRepository = balanceMonitoringTaskRepository;
            _blockchainService = blockchainServiceService;
            _blockLockDuration = settings.BlockLockDuration;
            _blockLockRepository = blockLockRepository;
            _chaosKitty = chaosKitty;
            _indexBlockStrategy = indexBlockStrategy;
            _log = logFactory.CreateLog(this);
            _minBlockNumberToIndex = settings.MinBlockNumberToIndex;
            _stateRepository = stateRepository;
            _transactionReceiptRepository = transactionReceiptRepository;
        }


        public async Task<BigInteger[]> GetNonIndexedBlocksAsync(
            int take)
        {
            try
            {
                var nonIndexedBlocks = new List<BigInteger>(take);
                var stateLock = await _stateRepository.WaitLockAsync();

                try
                {
                    var indexationState = await _stateRepository.GetOrCreateAsync();

                    // Update best block

                    var bestBlockNumber = await _blockchainService.GetBestTrustedBlockNumberAsync();

                    if (indexationState.TryToUpdateBestBlock(bestBlockNumber))
                    {
                        _chaosKitty.Meow("Failed to update indexation state.");

                        await _stateRepository.UpdateAsync(indexationState);

                        _log.Debug($"Best block updated to {bestBlockNumber}.");
                    }

                    // Get and clean up block locks

                    var locksExpiredOn = DateTime.UtcNow - _blockLockDuration;

                    var blockLocks = (await _blockLockRepository.GetAsync())
                        .ToList();

                    var expiredBlockLocks = blockLocks
                        .Where(x => x.LockedOn <= locksExpiredOn)
                        .ToList();

                    foreach (var @lock in expiredBlockLocks)
                    {
                        blockLocks.Remove(@lock);

                        _log.Debug($"Releasing expired indexation lock for block {@lock.BlockNumber}...");

                        await ReleaseBlockLockAsync(@lock.BlockNumber);
                    }

                    // Get non-indexed blocks
                    foreach (var blockNumber in indexationState.GetNonIndexedBlockNumbers())
                    {
                        if (blockNumber < _minBlockNumberToIndex)
                        {
                            break;
                        }
                        
                        if (blockLocks.All(x => x.BlockNumber != blockNumber))
                        {
                            try
                            {
                                await _blockLockRepository.InsertOrReplaceAsync(blockNumber);

                                nonIndexedBlocks.Add(blockNumber);
                            }
                            catch (Exception e)
                            {
                                _log.Warning
                                (
                                    $"Failed to set indexation lock for block {blockNumber}", 
                                    e,
                                    new { blockNumber }
                                );
                            }
                        }
                        else
                        {
                            continue;
                        }

                        if (nonIndexedBlocks.Count == take)
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    await stateLock.ReleaseAsync();
                }

                _log.Debug
                (
                    nonIndexedBlocks.Any()
                        ? $"Got non-indexed blocks [{string.Join(", ", nonIndexedBlocks)}]."
                        : "Non-indexed blocks not found."
                );

                return nonIndexedBlocks.ToArray();
            }
            catch (Exception e)
            {
                _log.Warning("Failed to get non indexed blocks.", e);

                return Array.Empty<BigInteger>();
            }
        }

        public async Task<BigInteger[]> IndexBlocksAsync(
            BigInteger[] blockNumbers)
        {
            try
            {
                var indexedBlocks = new ConcurrentBag<BigInteger>();
                var indexationTasks = blockNumbers.Select
                    (
                        blockNumber => Task.Run(async () =>
                        {
                            if (await IndexBlockAsync(blockNumber))
                            {
                                indexedBlocks.Add(blockNumber);
                            }
                            else
                            {
                                await ReleaseBlockLockAsync(blockNumber);
                            }
                        }))
                    .ToList();

                await Task.WhenAll(indexationTasks);

                return indexedBlocks.OrderBy(x => x).ToArray();
            }
            catch (Exception e)
            {
                _log.Warning($"Failed to index blocks [{string.Join(", ", blockNumbers)}].", e);

                return Array.Empty<BigInteger>();
            }
        }

        private async Task<bool> IndexBlockAsync(
            BigInteger blockNumber)
        {
            _log.Info
            (
                $"Block [{blockNumber}] indexation has been started.",
                new { blockNumber }
            );
            
            try
            {
                await _indexBlockStrategy.ExecuteAsync
                (
                    _balanceRepository,
                    _balanceMonitoringTaskRepository,
                    _transactionReceiptRepository,
                    _blockchainService,
                    blockNumber
                );
                
                _log.Info
                (
                    $"Block [{blockNumber}] has been indexed.",
                    new { blockNumber }
                );

                return true;
            }
            catch (Exception e)
            {
                _log.Warning
                (
                    $"Failed to index block [{blockNumber}].",
                    e,
                    new { blockNumber }
                );

                return false;
            }
        }
        
        public async Task MarkBlocksAsIndexed(
            BigInteger[] blockNumbers)
        {
            try
            {
                var stateLock = await _stateRepository.WaitLockAsync();

                try
                {
                    var indexationState = await _stateRepository.GetOrCreateAsync();
                    var stateUpdateIsNecessary = false;

                    foreach (var blockNumber in blockNumbers)
                    {
                        if (indexationState.TryToMarkBlockAsIndexed(blockNumber))
                        {
                            stateUpdateIsNecessary = true;
                        }
                    }

                    if (stateUpdateIsNecessary)
                    {
                        await _stateRepository.UpdateAsync(indexationState);
                    }
                }
                finally
                {
                    await stateLock.ReleaseAsync();
                }

                await Task.WhenAll(blockNumbers.Select(ReleaseBlockLockAsync));

                if (blockNumbers.Any())
                {
                    _log.Debug($"Blocks [{string.Join(", ", blockNumbers)}] has been marked as indexed.");
                }
            }
            catch (Exception e)
            {
                _log.Warning($"Failed to mark blocks [{string.Join(", ", blockNumbers)}] as indexed.", e);
            }
        }
        
        private async Task ReleaseBlockLockAsync(
            BigInteger blockNumber)
        {
            try
            {
                await _blockLockRepository.DeleteIfExistsAsync(blockNumber);
                
                _log.Debug($"Block indexation lock for block {blockNumber} has been released.");
            }
            catch (Exception e)
            {
                _log.Warning($"Failed to release indexation lock for block {blockNumber}.", e);
            }
        }

        public class Settings
        {
            public TimeSpan BlockLockDuration { get; set; }
            
            public BigInteger MinBlockNumberToIndex { get; set; }
        }
    }
}