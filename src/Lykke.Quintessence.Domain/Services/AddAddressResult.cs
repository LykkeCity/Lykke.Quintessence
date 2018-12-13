namespace Lykke.Quintessence.Domain.Services
{
    public abstract class AddAddressResult
    {
        public static HasAlreadyBeenAddedError HasAlreadyBeenAdded() 
            => new HasAlreadyBeenAddedError();
        
        public static SuccessResult Success() 
            => new SuccessResult();
        
        
        public class SuccessResult : AddAddressResult
        {
            internal SuccessResult()
            {
                
            }
        }

        public class HasAlreadyBeenAddedError : AddAddressResult
        {
            internal HasAlreadyBeenAddedError()
            {
                
            }
        }
    }
}
