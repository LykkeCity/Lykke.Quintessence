using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class RootstockNonceService : INonceService
    {
        private readonly IEthApiClient _ethApiClient;
        
        
        public RootstockNonceService(
            IEthApiClient ethApiClient)
        {
            _ethApiClient = ethApiClient;
        }

        
        public async Task<BigInteger> GetNextNonceAsync(
            string address)
        {
            return await _ethApiClient.GetTransactionCountAsync(address, true);
        }
    }
}