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

                if (transaction != null)
                {
                    var transactionChanged = false;
                    
                    // ReSharper disable once ConvertIfStatementToSwitchStatement
                    if (transaction.State == TransactionState.InProgress)
                    {
                        transactionChanged = await CheckTransactionCompletionStateAsync(transaction);
                    }

                    if (transaction.State == TransactionState.Completed || transaction.State == TransactionState.Failed)
                    {
                        transactionChanged = await CheckTransactionConfirmationStateAsync(transaction);
                    }

                    if (transactionChanged)
                    {
                        await _transactionRepository.UpdateAsync(transaction);
                    }

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (transaction.State)
                    {
                        case TransactionState.Built:
                            return true;
                        
                        case TransactionState.InProgress:
                            return false;
                        
                        case TransactionState.Completed:
                        case TransactionState.Failed:
                            return transaction.IsConfirmed;
                        
                        case TransactionState.Deleted:
                            return true;
                        
                        default:
                            throw new ArgumentException($"Transaction is in unknown state [{transaction.State}].");
                    }
                }
                else
                {
                    _log.Warning($"Transaction [{task.TransactionId}] does not exist.");

                    return true;
                }
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

        private async Task<bool> CheckTransactionCompletionStateAsync(
            Transaction transaction)
        {
            var transactionResult = await _blockchainService.GetTransactionResultAsync(transaction.Hash);
                    
            if (transactionResult != null)
            {
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
                    
                    _log.Info
                    (
                        !transactionResult.IsFailed
                            ? $"Transaction [{transaction.TransactionId}] succeeded in block {transactionResult.BlockNumber}."
                            : $"Transaction [{transaction.TransactionId}] failed in block {transactionResult.BlockNumber}.",
                        new { transactionId = transaction.TransactionId }
                    );

                    return true;
                }
            }
            else
            {
                _log.Warning($"Transaction [{transaction.TransactionId}] disappeared, after if has been broadcasted.");
            }

            return false;
        }

        private async Task<bool> CheckTransactionConfirmationStateAsync(
            Transaction transaction)
        {
            if (!transaction.IsConfirmed)
            {
                var bestTrustedBlockNumber = await _blockchainService.GetBestTrustedBlockNumberAsync();

                if (transaction.BlockNumber <= bestTrustedBlockNumber)
                {
                    var confirmationLevel = await _blockchainService.GetConfirmationLevel();
                    
                    transaction.OnConfirmed(confirmationLevel);
  
                    _log.Info($"Transaction [{transaction.TransactionId}] has been confirmed.");

                    return true;
                }
            }

            return false;
        }
    }
}