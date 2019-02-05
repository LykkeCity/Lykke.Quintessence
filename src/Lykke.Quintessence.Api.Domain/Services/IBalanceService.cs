using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IBalanceService
    {
        Task<bool> BeginObservationIfNotObservingAsync(
            [NotNull] string address);
        
        Task<bool> EndObservationIfObservingAsync(
            [NotNull] string address);

        Task<(IReadOnlyCollection<Balance> Balances, string ContinuationToken)> GetTransferableBalancesAsync(
            int take,
            [NotNull] string continuationToken);
    }
}