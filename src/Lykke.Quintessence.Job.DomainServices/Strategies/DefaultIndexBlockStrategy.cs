using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultIndexBlockStrategy : IIndexBlockStrategy
    {
        private readonly bool _indexOnlyOwnTransactions;
        
        public DefaultIndexBlockStrategy(
            bool indexOnlyOwnTransaction)
        {
            _indexOnlyOwnTransactions = indexOnlyOwnTransaction;
        }

        public async Task ExecuteAsync(
            IBalanceRepository balanceRepository,
            IBalanceMonitoringTaskRepository balanceMonitoringTaskRepository,
            ITransactionReceiptRepository transactionReceiptRepository,
            IBlockchainService blockchainService,
            BigInteger blockNumber)
        {
            var clearBlockTask = transactionReceiptRepository.ClearBlockAsync(blockNumber);
            
            var transactionReceipts = (await blockchainService.GetTransactionReceiptsAsync(blockNumber))
                .ToList();

            var affectedAddresses = transactionReceipts
                .Select(GetAddresses)
                .SelectMany(x => x)
                .Where(x => !string.IsNullOrEmpty(x) && x != "0x")
                .Distinct();
            
            var ownAddresses = new List<string>();

            foreach (var address in affectedAddresses)
            {
                var balance = await balanceRepository.TryGetAsync(address);

                if (balance != null)
                {
                    ownAddresses.Add(address);

                    if (balance.BlockNumber < blockNumber)
                    {
                        await balanceMonitoringTaskRepository.EnqueueAsync
                        (
                            new BalanceMonitoringTask(address)
                        );
                    }
                }
            }
            
            await clearBlockTask;

            if (_indexOnlyOwnTransactions)
            {
                var ownTransactionReceipts = transactionReceipts
                    .Where(x => ownAddresses.Contains(x.From) || ownAddresses.Contains(x.To));

                foreach (var receipt in ownTransactionReceipts)
                {
                    await transactionReceiptRepository.InsertOrReplaceAsync(receipt);
                }
            }
            else
            {
                foreach (var receipt in transactionReceipts)
                {
                    await transactionReceiptRepository.InsertOrReplaceAsync(receipt);
                }
            }
        }
        
        private static IEnumerable<string> GetAddresses(
            TransactionReceipt receipt)
        {
            yield return receipt.From;
            yield return receipt.To;
        }
    }
}