using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IGetTransactionReceiptsStrategy
    {
        Task<IEnumerable<TransactionReceipt>> ExecuteAsync(
            BigInteger blockNumber);
    }
}