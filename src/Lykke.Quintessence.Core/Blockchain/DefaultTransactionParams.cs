using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Core.Blockchain
{
    [PublicAPI]
    public class DefaultTransactionParams
    {
        public DefaultTransactionParams(
            BigInteger amount,
            string from,
            BigInteger gasAmount,
            BigInteger gasPrice,
            BigInteger nonce,
            string to)
        {
            Amount = amount;
            From = from;
            GasAmount = gasAmount;
            GasPrice = gasPrice;
            Nonce = nonce;
            To = to;
        }


        public BigInteger Amount { get; }
        
        public string From { get; }
        
        public BigInteger GasAmount { get; }
        
        public BigInteger GasPrice { get; }
        
        public BigInteger Nonce { get; }
        
        public string To { get; }
    }
}