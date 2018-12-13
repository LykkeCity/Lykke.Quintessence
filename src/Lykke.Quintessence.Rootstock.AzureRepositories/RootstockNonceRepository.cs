using System.Numerics;
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
    public class RootstockNonceRepository : IRootstockNonceRepository
    {
        private readonly INoSQLTableStorage<NonceEntity> _nonces;

        
        private RootstockNonceRepository(
            INoSQLTableStorage<NonceEntity> nonces)
        {
            _nonces = nonces;
        }

        public static IRootstockNonceRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            var nonces = AzureTableStorage<NonceEntity>.Create
            (
                connectionString,
                "Nonces",
                logFactory
            );
            
            return new RootstockNonceRepository(nonces);
        }
        
        public Task InsertOrReplaceAsync(
            string address,
            BigInteger nonce)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            return _nonces.InsertOrReplaceAsync(new NonceEntity
            {
                Value = nonce,

                PartitionKey = partitionKey,
                RowKey = rowKey
            });
        }

        public async Task<BigInteger?> TryGetAsync(
            string address)
        {
            var (partitionKey, rowKey) = GetKeys(address);

            var nonce = await _nonces.GetDataAsync
            (
                partition: partitionKey,
                row: rowKey
            );

            return nonce?.Value;
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