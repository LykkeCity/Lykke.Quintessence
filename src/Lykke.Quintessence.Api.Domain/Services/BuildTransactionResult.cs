using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    [PublicAPI]
    public abstract class BuildTransactionResult
    {
        public static Error AmountIsTooSmall()
            => new Error(BuildTransactionError.AmountIsTooSmall);
        
        public static Error BalanceIsNotEnough()
            => new Error(BuildTransactionError.BalanceIsNotEnough);
        
        public static Error GasAmountIsInvalid()
            => new Error(BuildTransactionError.GasAmountIsInvalid);
        
        public static Error TargetAddressIsInvalid()
            => new Error(BuildTransactionError.TargetAddressIsInvalid);
        
        public static Error TransactionHasBeenBroadcasted() 
            => new Error(BuildTransactionError.TransactionHasBeenDeleted);
        
        public static Error TransactionHasBeenDeleted()
            => new Error(BuildTransactionError.TransactionHasBeenDeleted);
        
        public static TransactionContext Success(string txData)
            => new TransactionContext(txData);
        
        
        public class TransactionContext : BuildTransactionResult
        {
            internal TransactionContext(
                string txData)
            {
                String = txData;
            }

            public string String { get; }
        }

        public class Error : BuildTransactionResult
        {
            internal Error(
                BuildTransactionError type)
            {
                Type = type;
            }
            
            public BuildTransactionError Type { get; }
        }
    }
}
