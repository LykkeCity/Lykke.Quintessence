using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultGetTransactionReceiptsStrategy : IGetTransactionReceiptsStrategy
    {
        private readonly IDetectContractStrategy _detectContractStrategy;
        private readonly IEthApiClient _ethApiClient;
        private readonly IParityApiClient _parityApiClient;
        private readonly string[] _valueTransferCallCodes = { "CREATE", "CALL", "CALLCODE", "DELEGATECALL", "SUICIDE" };


        public DefaultGetTransactionReceiptsStrategy(
            IDetectContractStrategy detectContractStrategy,
            IEthApiClient ethApiClient,
            IParityApiClient parityApiClient)
        {
            _detectContractStrategy = detectContractStrategy;
            _ethApiClient = ethApiClient;
            _parityApiClient = parityApiClient;
        }

        
        public async Task<IEnumerable<TransactionReceipt>> ExecuteAsync(
            BigInteger blockNumber)
        {
            var block = await _ethApiClient.GetBlockAsync
            (
                blockNumber: blockNumber,
                includeTransactions: true
            );

            var transactionReceipts = new List<TransactionReceipt>();

            if (block.Transactions != null)
            {
                foreach (var blockTransaction in block.Transactions)
                {
                    if (blockTransaction.To != null && await IsContractAsync(blockTransaction.To))
                    {
                        transactionReceipts.AddRange
                        (
                            await GetInternalTransactionReceiptsAsync
                            (
                                blockTransaction.TransactionHash,
                                block.Timestamp
                            )
                        );
                    }
                    else
                    {
                        transactionReceipts.Add(new TransactionReceipt
                        (
                            amount: blockTransaction.Value,
                            blockNumber: blockNumber,
                            from: blockTransaction.From,
                            hash: blockTransaction.TransactionHash,
                            index: 0,
                            timestamp: block.Timestamp,
                            to: blockTransaction.To
                        ));
                    }
                }
            }
            
            return transactionReceipts;
        }

        private async Task<bool> IsContractAsync(
            string address)
        {
            var code = await _ethApiClient.GetCodeAsync(address);

            return _detectContractStrategy.Execute(code);
        }
        
        private async Task<IEnumerable<TransactionReceipt>> GetInternalTransactionReceiptsAsync(
            string txHash,
            BigInteger timestamp)
        {
            var traces = (await _parityApiClient.GetTransactionTraces(txHash))
                .ToList();

            if (traces.Count > 0)
            {
                var result = new List<TransactionReceipt>();

                var valueTransferTraces = traces
                    .Where(x => _valueTransferCallCodes.Contains(x.Action.CallType, StringComparer.InvariantCultureIgnoreCase))
                    .Where(x => x.Action.Value != 0);

                var index = 0;
                
                foreach (var trace in valueTransferTraces)
                {
                    result.Add(new TransactionReceipt
                    (
                        amount: trace.Action.Value,
                        blockNumber: trace.BlockNumber,
                        from: trace.Action.From,
                        hash: trace.TransactionHash,
                        index: index++,
                        timestamp: timestamp,
                        to: trace.Action.To
                    ));
                }

                return result;
            }
            else
            {
                return Enumerable.Empty<TransactionReceipt>();
            }
        }
    }
}