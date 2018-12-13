using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Utils;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultSignService : ISignService
    {
        private readonly ITransactionBuilder _transactionBuilder;

        
        public DefaultSignService(
            ITransactionBuilder transactionBuilder)
        {
            _transactionBuilder = transactionBuilder;
        }

        
        public string SignTransaction(
            string encodedTxParams,
            string privateKey)
        {
            var serializedTxParams = encodedTxParams.HexToUTF8String();
            
            var rawTransaction = _transactionBuilder.BuildRawTransaction
            (
                serializedTxParams,
                privateKey
            );

            return rawTransaction.ToHex();
        }
    }
}