using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultTransactionMonitoringService : ITransactionMonitoringService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly ILog _log;
        private readonly ITransactionMonitoringTaskRepository _transactionMonitoringTaskRepository;
        private readonly ITransactionRepository _transactionRepository;

        
        public DefaultTransactionMonitoringService(
            IBlockchainService blockchainService,
            ILogFactory logFactory,
            ITransactionMonitoringTaskRepository transactionMonitoringTaskRepository,
            ITransactionRepository transactionRepository)
        {
            _blockchainService = blockchainService;
            _log = logFactory.CreateLog(this);
            _transactionMonitoringTaskRepository = transactionMonitoringTaskRepository;
            _transactionRepository = transactionRepository;
        }

        
        public async Task<bool> CheckAndUpdateStateAsync(
            TransactionMonitoringTask task)
        {
            try
            {
                var transaction = await _transactionRepository.TryGetAsync(task.TransactionId);

                if (transaction == null)
                {
                    _log.Warning($"Transaction [{task.TransactionId}] does not exist.");

                    return true;
                }
                
                if (transaction.State == TransactionState.Built || transaction.State == TransactionState.Deleted)
                {
                    // We should not check transaction state, until it has been broadcasted,
                    // or if it has been marked as deleted
                    
                    return true;
                }
                
                if (transaction.State == TransactionState.Completed || transaction.State == TransactionState.Failed)
                {
                    if (transaction.IsConfirmed)
                    {
                        // Transaction workflow has already been completed
                        
                        return true;
                    }
                    
                    var transactionResult = await _blockchainService.GetTransactionResultAsync(transaction.Hash);

                    if (transactionResult == null)
                    {
                        _log.Warning($"Transaction [{task.TransactionId}] disappeared, after if has been included in block.");

                        return true;
                    }
                    
                    var bestTrustedBlockNumber = await _blockchainService.GetBestTrustedBlockNumberAsync();

                    if (transactionResult.BlockNumber <= bestTrustedBlockNumber)
                    {
                        transaction.OnConfirmed(await _blockchainService.GetConfirmationLevel());

                        await _transactionRepository.UpdateAsync(transaction);
                        
                        _log.Info($"Transaction [{task.TransactionId}] has been confirmed.");
                        
                        return true;
                    }
                }

                if (transaction.State == TransactionState.InProgress)
                {
                    var transactionResult = await _blockchainService.GetTransactionResultAsync(transaction.Hash);
                    
                    if (transactionResult == null)
                    {
                        transaction.OnFailed
                        (
                            0,
                            "Transaction suddenly disappeared."
                        );

                        await _transactionRepository.UpdateAsync(transaction);
                        
                        _log.Warning($"Transaction [{task.TransactionId}] disappeared, after if has been broadcasted.");
                        
                        return true;
                    }
                    
                    if (transactionResult.IsCompleted)
                    {
                        if (!transactionResult.IsFailed)
                        {
                            transaction.OnSucceeded
                            (
                                transactionResult.BlockNumber
                            );
                        }
                        else
                        {
                            transaction.OnFailed
                            (
                                transactionResult.BlockNumber,
                                transactionResult.Error
                            );
                        }

                        await _transactionRepository.UpdateAsync(transaction);
                            
                        LogTransactionResult(task.TransactionId, transactionResult);

                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception e)
            {
                _log.Warning($"Failed to check and update transaction [{task.TransactionId}] state.", e);
                
                return false;
            }
        }

        public async Task CompleteMonitoringTaskAsync(
            TransactionMonitoringTask task)
        {
            try
            {
                await _transactionMonitoringTaskRepository.CompleteAsync(task);
            }
            catch (Exception e)
            {
                _log.Warning("Failed to complete transaction monitoring task.", e);
            }
        }

        public async Task<TransactionMonitoringTask> TryGetNextMonitoringTaskAsync()
        {
            try
            {
                return await _transactionMonitoringTaskRepository.TryGetAsync
                (
                    visibilityTimeout: TimeSpan.FromMinutes(1)
                );
            }
            catch (Exception e)
            {
                _log.Warning("Failed to get next transaction monitoring task.", e);

                return null;
            }
        }

        private void LogTransactionResult(
            Guid transactionId,
            TransactionResult transactionResult)
        {
            if (transactionResult.IsCompleted)
            {
                _log.Info
                (
                    !transactionResult.IsFailed
                        ? $"Transaction [{transactionId}] succeeded in block {transactionResult.BlockNumber}."
                        : $"Transaction [{transactionId}] failed in block {transactionResult.BlockNumber}.",
                    new { transactionId }
                );
            }
            else
            {
                _log.Debug($"Transaction [{transactionId}] is in progress.");
            }
        }
    }
}