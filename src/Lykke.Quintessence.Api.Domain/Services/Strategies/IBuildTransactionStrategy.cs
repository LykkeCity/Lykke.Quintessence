using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IBuildTransactionStrategy
    {
        Task<string> ExecuteAsync(
            Guid transactionId,
            string from,
            string to,
            BigInteger transactionAmount,
            BigInteger gasAmount,
            bool includeFee);
    }
}