using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultTryGetTransactionErrorStrategy : ITryGetTransactionErrorStrategy
    {
        private readonly IParityApiClient _parityApiClient;

        
        public DefaultTryGetTransactionErrorStrategy(
            IParityApiClient parityApiClient)
        {
            _parityApiClient = parityApiClient;
        }

        
        public async Task<string> ExecuteAsync(
            BigInteger? transactionStatus,
            string transactionHash)
        {
            if (transactionStatus.HasValue)
            {
                switch ((int)transactionStatus.Value)
                {
                    case 0:
                        return "Transaction failed";
                    case 1:
                        return null;
                    default:
                        throw new Exception($"Transaction [{transactionHash}] has unexpected [{transactionStatus.Value}] status.");
                }
            }
            else
            {
                return (await _parityApiClient.GetTransactionTraces(transactionHash))
                    .Select(x => x.Error)
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x));
            }
        }
    }
}