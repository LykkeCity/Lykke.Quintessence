using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultNonceService : INonceService
    {
        private readonly IParityApiClient _parityApiClient;

        
        public DefaultNonceService(
            IParityApiClient parityApiClient)
        {
            _parityApiClient = parityApiClient;
        }

        
        public Task<BigInteger> GetNextNonceAsync(
            string address)
        {
            return _parityApiClient.GetNextNonceAsync(address);
        }
    }
}