using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories.Entities;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBlockIndexationLockRepository : IBlockIndexationLockRepository
    {
        private readonly INoSQLTableStorage<BlockIndexationLockEntity> _locks;
        
        
        private DefaultBlockIndexationLockRepository(
            INoSQLTableStorage<BlockIndexationLockEntity> locks)
        {
            _locks = locks;
        }
        
        
        public static IBlockIndexationLockRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var locks = AzureTableStorage<BlockIndexationLockEntity>.Create
            (
                connectionString,
                "BlockIndexationLocks",
                logFactory
            );
            
            return new DefaultBlockIndexationLockRepository
            (
                locks: locks
            );
        }
        
        public Task DeleteIfExistsAsync(
            BigInteger blockNumber)
        {
            var (partitionKey, rowKey) = GetKeys(blockNumber);

            return _locks.DeleteIfExistAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey
            );
        }

        public async Task<IEnumerable<BlockIndexationLock>> GetAsync()
        {
            return (await _locks.GetDataAsync())
                .Select(x => new BlockIndexationLock
                (
                    blockNumber: x.BlockNumber,
                    lockedOn: x.LockedOn
                ));
        }

        public Task InsertOrReplaceAsync(
            BigInteger blockNumber)
        {
            var (partitionKey, rowKey) = GetKeys(blockNumber);

            return _locks.InsertOrReplaceAsync(new BlockIndexationLockEntity
            {
                BlockNumber = blockNumber,
                LockedOn = DateTime.UtcNow,

                PartitionKey = partitionKey,
                RowKey = rowKey
            });
        }
        
        #region Key Builders
        
        private static (string PartitionKey, string RowKey) GetKeys(
            BigInteger blockNumber)
        {
            // It is neither big, nor high-loaded table, so scaling by partition key is not necessary
            // Also, single partition allows us to optimise i/o performance with batch operations
            return (string.Empty, blockNumber.ToString());
        }
        
        #endregion
    }
}