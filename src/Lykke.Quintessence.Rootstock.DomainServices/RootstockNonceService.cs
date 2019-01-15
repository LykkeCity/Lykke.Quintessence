using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class RootstockNonceService : INonceService
    {
        private readonly IApiClient _apiClient;
        
        
        public RootstockNonceService(
            IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        
        public async Task<BigInteger> GetNextNonceAsync(
            string address)
        {
            return await _apiClient.GetTransactionCountAsync(address, true);
        }
    }
}