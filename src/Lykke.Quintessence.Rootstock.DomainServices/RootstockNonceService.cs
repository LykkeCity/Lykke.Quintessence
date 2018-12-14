using System;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class RootstockNonceService : IRootstockNonceService
    {
        private readonly IApiClient _apiClient;
        private readonly IRootstockNonceRepository _rootstockNonceRepository;

        
        public RootstockNonceService(
            IApiClient apiClient,
            IRootstockNonceRepository rootstockNonceRepository)
        {
            _apiClient = apiClient;
            _rootstockNonceRepository = rootstockNonceRepository;
        }

        
        public async Task<BigInteger> GetNextNonceAsync(
            string address)
        {
            var nextNonce = await _rootstockNonceRepository.TryGetAsync(address);

            if (nextNonce == null)
            {
                nextNonce = await _apiClient.GetTransactionCountAsync(address);

                await _rootstockNonceRepository.InsertOrReplaceAsync(address, nextNonce.Value);
            }
            
            return nextNonce.Value;
        }

        public async Task IncreaseNonceAsync(
            string address)
        {
            var nonce = await _rootstockNonceRepository.TryGetAsync(address);

            if (nonce == null)
            {
                throw new InvalidOperationException();
            }

            await _rootstockNonceRepository.InsertOrReplaceAsync(address, nonce.Value + 1);
        }
    }
}