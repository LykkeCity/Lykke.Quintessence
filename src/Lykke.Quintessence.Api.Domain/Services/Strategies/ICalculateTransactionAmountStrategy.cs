using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface ICalculateTransactionAmountStrategy
    {
        Task<TransactionAmountCalculationResult> ExecuteAsync(
            IBlockchainService blockchainService,
            string from,
            BigInteger transferAmount,
            BigInteger gasAmount,
            bool includeFee);
    }
}