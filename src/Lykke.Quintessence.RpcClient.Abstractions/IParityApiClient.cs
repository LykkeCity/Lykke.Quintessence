using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Quintessence.RpcClient.Models;

namespace Lykke.Quintessence.RpcClient
{
    public interface IParityApiClient
    {
        Task<BigInteger> GetNextNonceAsync(
            string address);

        Task<IEnumerable<TransactionTrace>> GetTransactionTraces(
            string transactionHash);
    }
}