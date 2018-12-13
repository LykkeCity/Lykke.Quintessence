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
        private readonly IEthApiClient _ethApiClient;
        private readonly IRootstockNonceService _nonceService;
        
        public RootstockBlockchainService(
            IDetectContractStrategy detectContractStrategy,
            IEthApiClient ethApiClient,
            IGetTransactionReceiptsStrategy getTransactionReceiptsStrategy,
            IRootstockNonceService nonceService,
            Settings settings) 
            
            : base(detectContractStrategy, ethApiClient, getTransactionReceiptsStrategy, nonceService, settings)
        {
            _ethApiClient = ethApiClient;
            _nonceService = nonceService;
        }

        public override async Task<string> BroadcastTransactionAsync(
            string signedTxData)
        {
            var serializedTransaction = signedTxData.HexToUTF8String();
            var transaction = JsonConvert.DeserializeObject<DefaultRawTransaction>(serializedTransaction);

            if (await _ethApiClient.GetTransactionAsync(transaction.Hash) != null)
            {
                return transaction.Hash;
            }
            
            await _ethApiClient.SendRawTransactionAsync(transaction.Data);
                
            for (var i = 0; i < 10; i++)
            {
                if (await _ethApiClient.GetTransactionAsync(transaction.Hash) != null)
                {
                    await _nonceService.IncreaseNonceAsync(transaction.From);
                    
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
    }
}