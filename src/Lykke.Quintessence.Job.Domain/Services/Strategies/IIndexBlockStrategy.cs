using System.Numerics;
using System.Threading.Tasks;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IIndexBlockStrategy
    {
        Task ExecuteAsync(IBalanceRepository balanceRepository,
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            ITransactionReceiptRepository transactionReceiptRepository,
            IBlockchainService blockchainService,
            BigInteger blockNumber);
    }
}