using System;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class RootstockTryGetTransactionErrorStrategy : ITryGetTransactionErrorStrategy
    {
        public Task<string> ExecuteAsync(
            BigInteger? transactionStatus,
            string transactionHash)
        {
            if (transactionStatus.HasValue)
            {
                switch ((int)transactionStatus.Value)
                {
                    case 0:
                        return Task.FromResult("Transaction failed");
                    case 1:
                        return Task.FromResult<string>(null);
                    default:
                        throw new Exception($"Transaction [{transactionHash}] has unexpected [{transactionStatus.Value}] status.");
                }
            }
            else
            {
                throw new Exception("Transaction status should be specified.");
            }
        }
    }
}