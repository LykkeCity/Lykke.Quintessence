using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class TransactionReceipt
    {
        public TransactionReceipt(
            BigInteger amount,
            BigInteger blockNumber,
            string from,
            string hash,
            BigInteger index,
            BigInteger timestamp,
            string to)
        {
            Amount = amount;
            BlockNumber = blockNumber;
            From = from;
            Hash = hash;
            Index = index;
            Timestamp = timestamp;
            To = to;
        }

        
        public BigInteger Amount { get; }
        
        public BigInteger BlockNumber { get; }
        
        public string From { get; }
        
        public string Hash { get; }
        
        public BigInteger Index { get; }
        
        public BigInteger Timestamp { get; }
        
        public string To { get; }
    }
}