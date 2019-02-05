using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface ITransactionReceiptRepository
    {
        Task ClearBlockAsync(
            BigInteger blockNumber);

        Task<string> CreateContinuationTokenAsync(
            [NotNull] string address,
            TransactionDirection direction,
            string afterHash);

        Task<(IEnumerable<TransactionReceipt> Transactions, string ContinuationToken)> GetAsync(
            [NotNull] string address,
            TransactionDirection direction,
            int take,
            [CanBeNull] string continuationToken);

        Task InsertOrReplaceAsync(
            [NotNull] TransactionReceipt receipt);
    }
}
