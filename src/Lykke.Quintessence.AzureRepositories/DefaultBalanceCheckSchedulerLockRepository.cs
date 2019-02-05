using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Blob;
using Lykke.Quintessence.Core.DistributedLock;
using Lykke.Quintessence.Domain.Repositories.Utils;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories
{
    public class DefaultBalanceCheckSchedulerLockRepository : IBalanceCheckSchedulerLockRepository
    {
        private const string Container = "balance-check-scheduler";
        private const string LockKey = ".lock";


        private readonly IBlobStorage _blobStorage;


        private DefaultBalanceCheckSchedulerLockRepository(
            IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public static IBalanceCheckSchedulerLockRepository Create(
            IReloadingManager<string> connectionString)
        {
            return new DefaultBalanceCheckSchedulerLockRepository
            (
                AzureBlobStorage.Create(connectionString)
            );
        }
        
        public Task<IDistributedLock> TryLockAsync()
        {
            return _blobStorage.TryLockAsync
            (
                Container,
                LockKey,
                TimeSpan.FromSeconds(60)
            );
        }
    }
}