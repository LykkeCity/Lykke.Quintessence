using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories.Entities;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultTransactionRepository : ITransactionRepository
    {
        private readonly INoSQLTableStorage<TransactionEntity> _transactions;

        
        private DefaultTransactionRepository(
            INoSQLTableStorage<TransactionEntity> transactions)
        {
            _transactions = transactions;
        }

        public static ITransactionRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var transactions = AzureTableStorage<TransactionEntity>.Create
            (
                connectionString,
                "Transactions",
                logFactory
            );
            
            return new DefaultTransactionRepository(transactions);
        }
        
        public async Task AddAsync(
            Transaction transaction)
        {
            var (partitionKey, rowKey) = GetTransactionKeys(transaction.TransactionId);
            
            var transactionEntity = new TransactionEntity
            {
                Amount = transaction.Amount,
                BlockNumber = transaction.BlockNumber,
                BroadcastedOn = transaction.BroadcastedOn,
                BuiltOn = transaction.BuiltOn,
                CompletedOn = transaction.CompletedOn,
                ConfirmationLevel = transaction.ConfirmationLevel,
                ConfirmedOn = transaction.ConfirmedOn,
                Data = transaction.Data,
                DeletedOn = transaction.DeletedOn,
                Error = transaction.Error,
                From = transaction.From,
                GasAmount = transaction.GasAmount,
                GasPrice = transaction.GasPrice,
                Hash = transaction.Hash,
                IncludeFee = transaction.IncludeFee,
                IsConfirmed = transaction.IsConfirmed,
                Nonce = transaction.Nonce,
                SignedData = transaction.SignedData,
                State = transaction.State,
                To = transaction.To,
                TransactionId = transaction.TransactionId,
                
                PartitionKey = partitionKey,
                RowKey = rowKey
            };

            await _transactions.InsertAsync(transactionEntity);
        }

        public async Task<Transaction> TryGetAsync(
            Guid transactionId)
        {
            var (partitionKey, rowKey) = GetTransactionKeys(transactionId);

            var transactionEntity = await _transactions.GetDataAsync
            (
                partition: partitionKey,
                row: rowKey
            );

            if (transactionEntity != null)
            {
                return new Transaction
                (
                    amount: transactionEntity.Amount,
                    blockNumber: transactionEntity.BlockNumber,
                    broadcastedOn: transactionEntity.BroadcastedOn,
                    builtOn: transactionEntity.BuiltOn,
                    completedOn: transactionEntity.CompletedOn,
                    confirmationLevel: transactionEntity.ConfirmationLevel,
                    confirmedOn: transactionEntity.ConfirmedOn,
                    data: transactionEntity.Data,
                    deletedOn: transactionEntity.DeletedOn,
                    error: transactionEntity.Error,
                    from: transactionEntity.From,
                    gasAmount: transactionEntity.GasAmount,
                    gasPrice: transactionEntity.GasPrice,
                    hash: transactionEntity.Hash,
                    includeFee: transactionEntity.IncludeFee,
                    isConfirmed: transactionEntity.IsConfirmed,
                    nonce: transactionEntity.Nonce,
                    signedData: transactionEntity.SignedData,
                    state: transactionEntity.State,
                    to: transactionEntity.To,
                    transactionId: transactionEntity.TransactionId
                );
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateAsync(
            Transaction transaction)
        {
            TransactionEntity MergeAction(TransactionEntity entity)
            {
                entity.Amount = transaction.Amount;
                entity.BlockNumber = transaction.BlockNumber;
                entity.BroadcastedOn = transaction.BroadcastedOn;
                entity.BuiltOn = transaction.BuiltOn;
                entity.CompletedOn = transaction.CompletedOn;
                entity.ConfirmationLevel = transaction.ConfirmationLevel;
                entity.ConfirmedOn = transaction.ConfirmedOn;
                entity.Data = transaction.Data;
                entity.DeletedOn = transaction.DeletedOn;
                entity.Error = transaction.Error;
                entity.From = transaction.From;
                entity.GasAmount = transaction.GasAmount;
                entity.GasPrice = transaction.GasPrice;
                entity.Hash = transaction.Hash;
                entity.IncludeFee = transaction.IncludeFee;
                entity.IsConfirmed = transaction.IsConfirmed;
                entity.Nonce = transaction.Nonce;
                entity.SignedData = transaction.SignedData;
                entity.State = transaction.State;
                entity.To = transaction.To;
                entity.TransactionId = transaction.TransactionId;

                return entity;
            }

            var (partitionKey, rowKey) = GetTransactionKeys(transaction.TransactionId);

            await _transactions.MergeAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey,
                mergeAction: MergeAction
            );
        }
        
        #region Key Builders
        
        private static (string PartitionKey, string RowKey) GetTransactionKeys(
            Guid transactionId)
        {
            return (transactionId.ToString().CalculateHexHash32(3), transactionId.ToString());
        }
        
        #endregion
    }
}