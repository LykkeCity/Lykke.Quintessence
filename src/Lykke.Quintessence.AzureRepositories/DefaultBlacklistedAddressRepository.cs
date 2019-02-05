using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories.Entities;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBlacklistedAddressRepository : IBlacklistedAddressRepository
    {
        private readonly INoSQLTableStorage<BlacklistedAddressEntity> _blacklistedAddresses;

        private DefaultBlacklistedAddressRepository(
            INoSQLTableStorage<BlacklistedAddressEntity> blacklistedAddresses)
        {
            _blacklistedAddresses = blacklistedAddresses;
        }

        public static IBlacklistedAddressRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var blacklistedAddresses = AzureTableStorage<BlacklistedAddressEntity>.Create
            (
                connectionString,
                "BlacklistedAddresses",
                logFactory
            );
            
            return new DefaultBlacklistedAddressRepository(blacklistedAddresses);
        }
        
        
        public Task<bool> AddIfNotExistsAsync(
            string address,
            string reason)
        {
            var (partitionKey, rowKey) = GetKeys(address);
            
            return _blacklistedAddresses.TryInsertAsync
            (
                new BlacklistedAddressEntity
                {
                    Reason = reason,
                    
                    PartitionKey = partitionKey,
                    RowKey = rowKey
                }
            );
        }

        public async Task<bool> ContainsAsync(
            string address)
        {
            var blacklistedAddress = await TryGetAsync(address);
            
            return blacklistedAddress != null;
        }
        
        public async Task<(IReadOnlyCollection<BlacklistedAddress> Addresses, string ContinuationToken)> GetAllAsync(
            int take,
            string continuationToken)
        {
            IEnumerable<BlacklistedAddressEntity> addresses;
            
            (addresses, continuationToken) = await _blacklistedAddresses.GetDataWithContinuationTokenAsync(take, continuationToken);

            return (addresses.Select(x => new BlacklistedAddress(x.RowKey, x.Reason)).ToImmutableList(), continuationToken);
        }

        public async Task<BlacklistedAddress> TryGetAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            var entity = await _blacklistedAddresses.GetDataAsync
            (
                partition: partitionKey,
                row: rowKey
            );

            if (entity != null)
            {
                return new BlacklistedAddress
                (
                    address: entity.RowKey,
                    blacklistingReason: entity.Reason
                );
            }
            else
            {
                return null;
            }
        }
        
        public Task<bool> RemoveIfExistsAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            return _blacklistedAddresses.DeleteIfExistAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey
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