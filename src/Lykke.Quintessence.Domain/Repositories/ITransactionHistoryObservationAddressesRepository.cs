using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface ITransactionHistoryObservationAddressesRepository
    {
        Task<bool> TryAddToIncomingHistoryObservationList(
            string address);
        
        Task<bool> TryAddToOutgoingHistoryObservationList(
            string address);
        
        Task<bool> TryDeleteFromIncomingHistoryObservationList(
            string address);
        
        Task<bool> TryDeleteFromOutgoingHistoryObservationList(
            string address);
    }
}