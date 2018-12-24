using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface ITransactionHistoryService
    {
        Task<bool> BeginIncomingHistoryObservationIfNotObservingAsync(
            [NotNull] string address);
        
        Task<bool> BeginOutgoingHistoryObservationIfNotObservingAsync(
            [NotNull] string address);
        
        Task<bool> EndIncomingHistoryObservationIfObservingAsync(
            [NotNull] string address);
        
        Task<bool> EndOutgoingHistoryObservationIfObservingAsync(
            [NotNull] string address);
        
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