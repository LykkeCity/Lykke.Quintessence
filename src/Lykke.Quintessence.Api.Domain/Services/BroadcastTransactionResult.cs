using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    [PublicAPI]
    public abstract class BroadcastTransactionResult
    {
        public static Error BalanceIsNotEnough()
            => new Error(BroadcastTransactionError.BalanceIsNotEnough);
        
        public static Error TransactionCanNotBeBroadcasted() 
            => new Error(BroadcastTransactionError.TransactionCanNotBeBroadcasted);
        
        public static Error TransactionHasBeenBroadcasted() 
            => new Error(BroadcastTransactionError.TransactionHasBeenBroadcasted);
        
        public static Error TransactionHasBeenDeleted() 
            => new Error(BroadcastTransactionError.TransactionHasBeenDeleted);
        
        public static Error TransactionShouldBeRebuilt() 
            => new Error(BroadcastTransactionError.TransactionShouldBeRebuilt);
        
        public static Error TransactionHasNotBeenFound() 
            => new Error(BroadcastTransactionError.TransactionHasNotBeenFound);
        
        public static TransactionHash Success(string hash)
            => new TransactionHash(hash);
        
        
        public sealed class TransactionHash : BroadcastTransactionResult
        {
            internal TransactionHash(
                string hash)
            {
                String = hash;
            }
            
            public string String { get; }
        }

        public sealed class Error : BroadcastTransactionResult
        {
            internal Error(
                BroadcastTransactionError type)
            {
                Type = type;
            }

            public BroadcastTransactionError Type { get; }
        }
    }
}
