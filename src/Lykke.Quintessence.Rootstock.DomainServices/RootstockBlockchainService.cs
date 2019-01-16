using System;
using System.Threading.Tasks;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain.Services
{
    public class RootstockBlockchainService : DefaultBlockchainService
    {
        private readonly IApiClient _apiClient;
        
        public RootstockBlockchainService(
            IDetectContractStrategy detectContractStrategy,
            IApiClient apiClient,
            IGetTransactionReceiptsStrategy getTransactionReceiptsStrategy,
            INonceService nonceService,
            ITryGetTransactionErrorStrategy tryGetTransactionErrorStrategy,
            Settings settings) 
            
            : base(detectContractStrategy, apiClient, getTransactionReceiptsStrategy, nonceService, tryGetTransactionErrorStrategy, settings)
        {
            _apiClient = apiClient;
        }

        public override async Task<string> BroadcastTransactionAsync(
            string signedTxData)
        {
            var serializedTransaction = signedTxData.HexToUTF8String();
            var transaction = JsonConvert.DeserializeObject<DefaultRawTransaction>(serializedTransaction);

            if (await _apiClient.GetTransactionAsync(transaction.Hash) != null)
            {
                return transaction.Hash;
            }


            var transactionHasBeenBroadcasted = false;
            
            for (var i = 0; i < 30; i++)
            {
                var pendingTransactionsCount = await _apiClient.GetTransactionCountAsync(transaction.From, true) -
                                               await _apiClient.GetTransactionCountAsync(transaction.From, false);

                if (pendingTransactionsCount < 4)
                {
                    await _apiClient.SendRawTransactionAsync(transaction.Data);
                    
                    transactionHasBeenBroadcasted = true;
                }
                else
                {
                    await Task.Delay(1000);
                }
            }

            if (transactionHasBeenBroadcasted)
            {
                for (var i = 0; i < 10; i++)
                {
                    if (await _apiClient.GetTransactionAsync(transaction.Hash) != null)
                    {
                        return transaction.Hash;
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
             
                throw new Exception
                (
                    $"Transaction [{transaction.Hash}] has been broadcasted, but did not appear in mempool in the specified period of time."
                );
            }
            else
            {
                throw new Exception
                (
                    $"Transaction [{transaction.Hash}] has not been broadcasted."
                );
            }
        }
    }
}