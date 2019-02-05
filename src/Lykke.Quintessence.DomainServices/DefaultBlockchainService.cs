using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.Domain.Services.Utils;
using Lykke.Quintessence.RpcClient;
using Lykke.Quintessence.RpcClient.Exceptions;
using Lykke.SettingsReader;
using Newtonsoft.Json;


namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultBlockchainService : IBlockchainService
    {
        private readonly SettingManager<int> _confirmationLevel;
        private readonly IDetectContractStrategy _detectContractStrategy;
        private readonly IEthApiClient _ethApiClient;
        private readonly SettingManager<string, (BigInteger, BigInteger)> _gasPriceRange;
        private readonly IGetTransactionReceiptsStrategy _getTransactionReceiptsStrategy;
        private readonly ITryGetTransactionErrorStrategy _tryGetTransactionErrorStrategy;

        
        public DefaultBlockchainService(
            IEthApiClient ethApiClient,
            IDetectContractStrategy detectContractStrategy,
            IGetTransactionReceiptsStrategy getTransactionReceiptsStrategy,
            ITryGetTransactionErrorStrategy tryGetTransactionErrorStrategy,
            Settings settings)
        {
            _ethApiClient = ethApiClient;
            _detectContractStrategy = detectContractStrategy;
            _getTransactionReceiptsStrategy = getTransactionReceiptsStrategy;
            _tryGetTransactionErrorStrategy = tryGetTransactionErrorStrategy;
            
            _confirmationLevel = new SettingManager<int>
            (
                settings.ConfirmationLevel,
                TimeSpan.FromMinutes(1)
            );
            
            _gasPriceRange = new SettingManager<string, (BigInteger, BigInteger)>
            (
                settings.GasPriceRange,
                ParseGasPriceRange,
                TimeSpan.FromMinutes(1)
            );
        }

        public virtual async Task<string> BroadcastTransactionAsync(
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

        public virtual string BuildTransaction(
            string from,
            string to,
            BigInteger amount,
            BigInteger gasAmount,
            BigInteger gasPrice,
            BigInteger nonce)
        {
            var transactionParams = JsonConvert.SerializeObject(new DefaultTransactionParams
            (
                amount: amount,
                from: from,
                gasAmount: gasAmount,
                gasPrice: gasPrice,
                nonce: nonce,
                to: to
            ));

            return transactionParams.ToHex();
        }

        public virtual async Task<BigInteger> EstimateGasPriceAsync()
        {
            var (minGasPrice, maxGasPrice) = await _gasPriceRange.GetValueAsync();

            var estimatedGasPrice = await _ethApiClient.GetGasPriceAsync();

            if (estimatedGasPrice >= maxGasPrice)
            {
                return maxGasPrice;
            }
            else if (estimatedGasPrice <= minGasPrice)
            {
                return minGasPrice;
            }
            else
            {
                return estimatedGasPrice;
            }
        }

        public virtual Task<BigInteger> GetBalanceAsync(
            string address)
        {
            return _ethApiClient.GetBalanceAsync(address);
        }

        public virtual Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber)
        {
            return _ethApiClient.GetBalanceAsync(address, blockNumber);
        }

        public virtual Task<int> GetConfirmationLevel()
        {
            return _confirmationLevel.GetValueAsync();
        }
        
        public virtual async Task<BigInteger> GetBestTrustedBlockNumberAsync()
        {
            var bestBlockNumber = await _ethApiClient.GetBestBlockNumberAsync();
            var confirmationLevel = await _confirmationLevel.GetValueAsync();

            return bestBlockNumber - confirmationLevel;
        }

        public virtual Task<IEnumerable<TransactionReceipt>> GetTransactionReceiptsAsync(
            BigInteger blockNumber)
        {
            return _getTransactionReceiptsStrategy.ExecuteAsync(blockNumber);
        }

        public virtual async Task<TransactionResult> GetTransactionResultAsync(
            string hash)
        {
            var transactionReceipt = await _ethApiClient.GetTransactionReceiptAsync(hash);

            if (transactionReceipt?.BlockHash != null && transactionReceipt.BlockNumber != null)
            {
                var blockNumber = transactionReceipt.BlockNumber.Value;
                var error = await _tryGetTransactionErrorStrategy.ExecuteAsync(transactionReceipt.Status, hash);
                
                return new TransactionResult
                (
                    blockNumber: blockNumber,
                    error: error,
                    isCompleted: true,
                    isFailed: !string.IsNullOrEmpty(error)
                );
            }
            else
            {
                return null;
            }
        }

        public virtual async Task<bool> IsContractAsync(
            string address)
        {
            var code = await _ethApiClient.GetCodeAsync(address);

            return _detectContractStrategy.Execute(code);
        }

        public virtual async Task<BigInteger?> TryEstimateGasAmountAsync(
            string from,
            string to,
            BigInteger amount)
        {
            try
            {
                return await _ethApiClient.EstimateGasAmountAsync
                (
                    from: from,
                    to: to,
                    transferAmount: amount
                );
            }
            catch (GasAmountEstimationException)
            {
                return null;
            }
        }

        private static (BigInteger, BigInteger) ParseGasPriceRange(
            string source)
        {
            var minAndMagGasPrices = source.Split('-');
            var minGasPrice = BigInteger.Parse(minAndMagGasPrices[0]);
            var maxGasPrice = BigInteger.Parse(minAndMagGasPrices[1]);

            return (minGasPrice, maxGasPrice);
        }


        public class Settings
        {
            public IReloadingManager<int> ConfirmationLevel { get; set; }
            
            public IReloadingManager<string> GasPriceRange { get; set; }
        }
    }
}