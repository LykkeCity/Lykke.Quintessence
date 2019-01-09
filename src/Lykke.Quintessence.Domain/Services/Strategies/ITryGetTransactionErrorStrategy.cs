using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface ITryGetTransactionErrorStrategy
    {
        Task<string> ExecuteAsync(
            BigInteger? transactionStatus,
            string transactionHash);
    }
}