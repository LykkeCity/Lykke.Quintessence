﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IBalanceRepository
    {
        Task<bool> CreateIfNotExistsAsync(
            [NotNull] string address);
        
        Task<bool> DeleteIfExistsAsync(
            [NotNull] string address);
        
        Task<bool> ExistsAsync(
            [NotNull] string address);

        Task<(IEnumerable<Balance> Balances, string ContinuationToken)> GetAllTransferableBalancesAsync(
            int take,
            [CanBeNull] string continuationToken);

        Task<Balance> TryGetAsync(
            [NotNull] string address);

        Task UpdateSafelyAsync(
            [NotNull] Balance balance);
    }
}
