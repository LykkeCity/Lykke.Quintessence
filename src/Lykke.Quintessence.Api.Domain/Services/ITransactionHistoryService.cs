using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface ITransactionHistoryService
    {
        [ItemNotNull]
        Task<IEnumerable<TransactionReceipt>> GetIncomingHistoryAsync(
            [NotNull] string address,
            int take,
            string afterHash);
        
        [ItemNotNull]
        Task<IEnumerable<TransactionReceipt>> GetOutgoingHistoryAsync(
            [NotNull] string address,
            int take,
            string afterHash);
    }
}