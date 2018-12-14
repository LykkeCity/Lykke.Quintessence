using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class RootstockGetTransactionReceiptsStrategy : IGetTransactionReceiptsStrategy
    {
        private readonly IApiClient _apiClient;

        
        public RootstockGetTransactionReceiptsStrategy(
            IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        
        public async Task<IEnumerable<TransactionReceipt>> ExecuteAsync(
            BigInteger blockNumber)
        {
            var block = await _apiClient.GetBlockAsync
            (
                blockNumber: blockNumber,
                includeTransactions: true
            );

            var transactionReceipts =  block.Transactions?.Select(x => new TransactionReceipt
            (
                amount: x.Value,
                blockNumber: blockNumber,
                from: x.From,
                hash: x.TransactionHash,
                index: x.TransactionIndex ?? 0,
                timestamp: block.Timestamp,
                to: x.To
            ));

            return transactionReceipts ?? Enumerable.Empty<TransactionReceipt>();
        }
    }
}