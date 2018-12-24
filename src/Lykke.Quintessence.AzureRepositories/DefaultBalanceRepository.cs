using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories.Entities;
using Lykke.SettingsReader;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBalanceRepository : IBalanceRepository
    {
        private readonly INoSQLTableStorage<BalanceEntity> _balances;


        private DefaultBalanceRepository(
            INoSQLTableStorage<BalanceEntity> balances)
        {
            _balances = balances;
        }

        public static IBalanceRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var observableAccountStates = AzureTableStorage<BalanceEntity>.Create
            (
                connectionStringManager: connectionString,
                tableName: "ObservableBalances",
                logFactory: logFactory
            );
            
            return new DefaultBalanceRepository(observableAccountStates);
        }
        
        
        public Task<bool> CreateIfNotExistsAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            var entity = new BalanceEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey
            };
            
            return _balances.CreateIfNotExistsAsync(entity);
        }

        public Task<bool> DeleteIfExistsAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);
            
            return _balances.DeleteIfExistAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey
            );
        }

        public async Task<bool> ExistsAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);
            
            var entity = await _balances.GetDataAsync
            (
                partition: partitionKey,
                row: rowKey
            );

            return entity != null;
        }

        public async Task<(IEnumerable<Balance> Balances, string ContinuationToken)> GetAllTransferableBalancesAsync(
            int take,
            string continuationToken)
        {
            var filterCondition = TableQuery.GenerateFilterCondition(nameof(BalanceEntity.Amount), QueryComparisons.NotEqual, "0");
            
            var rangeQuery = new TableQuery<BalanceEntity>()
                .Where(filterCondition);

            var (entities, newContinuationToken) = await _balances
                .GetDataWithContinuationTokenAsync(rangeQuery, take, continuationToken);

            var balances = entities.Select(x => new Balance
            (
                address: x.RowKey,
                amount: x.Amount,
                blockNumber: x.BlockNumber
            ));

            return (balances, newContinuationToken);
        }

        public async Task<Balance> TryGetAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            var entity = await _balances.GetDataAsync(partition: partitionKey, row: rowKey);

            if (entity != null)
            {
                return new Balance
                (
                    address: entity.RowKey,
                    amount: entity.Amount,
                    blockNumber: entity.BlockNumber
                );
            }
            else
            {
                return null;
            }
        }

        public Task UpdateSafelyAsync(
            Balance balance)
        {
            BalanceEntity Merge(BalanceEntity entity)
            {
                if (balance.BlockNumber > entity.BlockNumber)
                {
                    entity.Amount = balance.Amount;
                    entity.BlockNumber = balance.BlockNumber;
                }
                
                return entity;
            }

            var (partitionKey, rowKey) = GetKeys(balance.Address);
            
            return _balances.MergeAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey,
                mergeAction: Merge
            );
        }
        
        #region Key Builders
        
        private static (string, string) GetKeys(
            string address)
        {
            var partitionKey = address.CalculateHexHash32(3);
            var rowKey = address;

            return (partitionKey, rowKey);
        }
        
        #endregion
    }
}