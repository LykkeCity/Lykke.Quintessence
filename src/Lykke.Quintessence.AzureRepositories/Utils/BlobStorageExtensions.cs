using System;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.DistributedLock;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;

namespace Lykke.Quintessence.Domain.Repositories.Utils
{
    [PublicAPI]
    public static class BlobStorageExtensions
    {
        public static async Task<IDistributedLock> TryLockAsync(
            this IBlobStorage blobStorage,
            string container,
            string key,
            TimeSpan? lockDuration)
        {
            try
            {
                var leaseId = await blobStorage.AcquireLeaseAsync
                (
                    container: container,
                    key: key,
                    leaseTime: lockDuration
                );

                return new BlobLeaseBasedLock
                (
                    blobStorage: blobStorage,
                    container: container,
                    key: key,
                    leaseId: leaseId,
                    lockDuration: lockDuration
                );
            }
            catch (StorageException e) when (e.RequestInformation.HttpStatusCode == StatusCodes.Status409Conflict)
            {
                return null;
            }
        }
        
        public static async Task<IDistributedLock> WaitLockAsync(
            this IBlobStorage blobStorage,
            string container,
            string key,
            TimeSpan? lockDuration)
        {
            while (true)
            {
                var @lock = await blobStorage.TryLockAsync(container, key, lockDuration);

                if (@lock != null)
                {
                    return @lock;
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }

        private class BlobLeaseBasedLock : IDistributedLock
        {
            private readonly IBlobStorage _blobStorage;
            private readonly string _container;
            private readonly string _key;
            private readonly string _leaseId;
            private readonly long? _lockDuration;
            
            private DateTime _renewAfter;
            
            public BlobLeaseBasedLock(
                IBlobStorage blobStorage,
                string container,
                string key,
                string leaseId,
                TimeSpan? lockDuration)
            {
                _blobStorage = blobStorage;
                _container = container;
                _key = key;
                _leaseId = leaseId;
                _lockDuration = lockDuration?.Ticks;
                
                UpdateRenewAfter();
            }
            
            public async Task ReleaseAsync()
            {
                await  _blobStorage.ReleaseLeaseAsync
                (
                    container: _container,
                    key: _key,
                    leaseId: _leaseId
                );
            }
            
            public async Task RenewIfNecessaryAsync()
            {
                if (DateTime.UtcNow > _renewAfter)
                {
                    await _blobStorage.RenewLeaseAsync
                    (
                        container: _container,
                        key: _key,
                        leaseId: _leaseId
                    );
                        
                    UpdateRenewAfter();
                }
            }
            
            private void UpdateRenewAfter()
            {
                _renewAfter = _lockDuration.HasValue 
                            ? DateTime.UtcNow.AddTicks(_lockDuration.Value / 2) 
                            : DateTime.MaxValue;
            }
        }
    }
}