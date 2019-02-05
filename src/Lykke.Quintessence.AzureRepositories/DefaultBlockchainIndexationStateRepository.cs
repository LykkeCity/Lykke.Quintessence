using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Blob;
using Lykke.Quintessence.Core.DistributedLock;
using Lykke.Quintessence.Domain.Repositories.Utils;
using Lykke.SettingsReader;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBlockchainIndexationStateRepository : IBlockchainIndexationStateRepository
    {
        private const string Container = "blockchain-indexation-state";
        private const string DataKey = ".json";
        private const string LockKey = ".lock";


        private readonly IBlobStorage _blobStorage;


        private DefaultBlockchainIndexationStateRepository(
            IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public static IBlockchainIndexationStateRepository Create(
            IReloadingManager<string> connectionString)
        {
            return new DefaultBlockchainIndexationStateRepository
            (
                AzureBlobStorage.Create(connectionString)
            );
        }
        
        public async Task<BlockchainIndexationState> GetOrCreateAsync()
        {
            if (await _blobStorage.HasBlobAsync(Container, DataKey))
            {
                var stateJson = await _blobStorage.GetAsTextAsync(Container, DataKey);
                
                return BlockchainIndexationState.Restore
                (
                    JsonConvert.DeserializeObject<IEnumerable<BlockIntervalIndexationState>>(stateJson)
                );
            }
            else
            {
                return BlockchainIndexationState.Create();
            }
        }

        public async Task UpdateAsync(
            BlockchainIndexationState state)
        {
            var stateJson = JsonConvert.SerializeObject(state);

            await _blobStorage.SaveBlobAsync
            (
                container: Container,
                key: DataKey,
                blob: Encoding.UTF8.GetBytes(stateJson)
            );
        }

        public Task<IDistributedLock> WaitLockAsync()
        {
            return _blobStorage.WaitLockAsync
            (
                container: Container,
                key: LockKey,
                lockDuration: TimeSpan.FromSeconds(60)
            );
        }
    }
}