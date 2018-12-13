using System;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IBuildTransactionStrategy
    {
        Task<string> ExecuteAsync(
            ITransactionRepository transactionRepository,
            IBlockchainService blockchainService,
            Guid transactionId,
            string from,
            string to,
            BigInteger transactionAmount,
            BigInteger gasAmount,
            bool includeFee);
    }
}