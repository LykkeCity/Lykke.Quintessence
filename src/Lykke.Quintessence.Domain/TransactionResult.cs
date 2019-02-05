using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class TransactionResult
    {
        public TransactionResult(
            BigInteger blockNumber,
            string error,
            bool isCompleted,
            bool isFailed)
        {
            BlockNumber = blockNumber;
            Error = error;
            IsCompleted = isCompleted;
            IsFailed = isFailed;
        }

        
        public BigInteger BlockNumber { get; }
        
        public string Error { get; }
        
        public bool IsCompleted { get; }
        
        public bool IsFailed { get; }
    }
}