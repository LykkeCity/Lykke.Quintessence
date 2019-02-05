using System.Collections.Generic;
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
        private readonly IEthApiClient _ethApiClient;

        
        public RootstockGetTransactionReceiptsStrategy(
            IEthApiClient ethApiClient)
        {
            _ethApiClient = ethApiClient;
        }

        
        public async Task<IEnumerable<TransactionReceipt>> ExecuteAsync(
            BigInteger blockNumber)
        {
            var block = await _ethApiClient.GetBlockAsync
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