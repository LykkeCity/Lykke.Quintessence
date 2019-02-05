using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Domain.Repositories.Entities;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    [UsedImplicitly]
    public class DefaultTransactionHistoryObservationAddressesRepository : ITransactionHistoryObservationAddressesRepository
    {
        private readonly INoSQLTableStorage<TransactionHistoryObservationAddressEntity> _addresses;

        private DefaultTransactionHistoryObservationAddressesRepository(
            INoSQLTableStorage<TransactionHistoryObservationAddressEntity> addresses)
        {
            _addresses = addresses;
        }
        
        public static ITransactionHistoryObservationAddressesRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var transactions = AzureTableStorage<TransactionHistoryObservationAddressEntity>.Create
            (
                connectionString,
                "TransactionHistoryObservationAddresses",
                logFactory
            );
            
            return new DefaultTransactionHistoryObservationAddressesRepository(transactions);
        }

        
        public Task<bool> TryAddToIncomingHistoryObservationList(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address, true);

            return _addresses.TryInsertAsync(new TransactionHistoryObservationAddressEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey
            });
        }

        public Task<bool> TryAddToOutgoingHistoryObservationList(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address, false);
            
            return _addresses.TryInsertAsync(new TransactionHistoryObservationAddressEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey
            });
        }

        public Task<bool> TryDeleteFromIncomingHistoryObservationList(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address, true);

            return _addresses.DeleteIfExistAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey
            );
        }

        public Task<bool> TryDeleteFromOutgoingHistoryObservationList(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address, false);
            
            return _addresses.DeleteIfExistAsync
            (
                partitionKey: partitionKey,
                rowKey: rowKey
            );
        }
        
        #region Key Builders
        
        private static (string, string) GetKeys(
            string address,
            bool forIncomingHistory)
        {
            var prefix = forIncomingHistory ? "incoming" : "outgoing";
            var partitionKey = $"{prefix}-{address.CalculateHexHash32(3)}";
            var rowKey = address;

            return (partitionKey, rowKey);
        }
        
        #endregion
    }
}