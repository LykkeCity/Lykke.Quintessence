using System.Numerics;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain.Strategies;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.Utils;
using Nethereum.RLP;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Core.Blockchain
{
    [UsedImplicitly]
    public class DefaultTransactionBuilder : ITransactionBuilder
    {
        private readonly IBuildRawTransactionStrategy _buildRawTransactionStrategy;
        private readonly IHashCalculator _hashCalculator;

        
        public DefaultTransactionBuilder(
            IBuildRawTransactionStrategy buildRawTransactionStrategy,
            IHashCalculator hashCalculator)
        {
            _buildRawTransactionStrategy = buildRawTransactionStrategy;
            _hashCalculator = hashCalculator;
        }


        public string BuildRawTransaction(
            string serializedTxParams,
            string privateKey)
        {
            var txParams = JsonConvert.DeserializeObject<DefaultTransactionParams>(serializedTxParams);
            
            var transactionElements = GetOrderedTransactionElements
            (
                to: txParams.To,
                amount: txParams.Amount,
                nonce: txParams.Nonce,
                gasPrice: txParams.GasPrice,
                gasLimit: txParams.GasAmount,
                data: null
            );

            var data = _buildRawTransactionStrategy.Execute
            (
                transactionElements,
                privateKey.HexToByteArray()
            );

            var rawTransaction = new DefaultRawTransaction
            (
                data: data.ToHexString(),
                from: txParams.From,
                hash: _hashCalculator.Sum(data).ToHexString()
            );
            
            return JsonConvert.SerializeObject(rawTransaction);
        }
        
        private static byte[][] GetOrderedTransactionElements(
            string to,
            BigInteger amount,
            BigInteger nonce,
            BigInteger gasPrice,
            BigInteger gasLimit,
            string data)
        {
            return new[]
            {
                nonce.ToBytesForRLPEncoding(),
                gasPrice.ToBytesForRLPEncoding(),
                gasLimit.ToBytesForRLPEncoding(),
                to.HexToByteArray(),
                amount.ToBytesForRLPEncoding(),
                data.HexToByteArray()
            };
        }
    }
}