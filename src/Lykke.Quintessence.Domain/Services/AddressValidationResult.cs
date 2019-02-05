namespace Lykke.Quintessence.Domain.Services
{
    public abstract class AddressValidationResult
    {
        public static AddressValidationResult AddressIsValid()
            => new Success();
        
        public static AddressValidationResult AddressIsInvalid(AddressValidationError error)
            => new Error(error);


        public sealed class Error : AddressValidationResult
        {
            internal Error(
                AddressValidationError type)
            {
                Type = type;
            }
            
            public AddressValidationError Type { get; }
        }
        
        public sealed class Success : AddressValidationResult
        {
            internal Success()
            {
                
            }
        }
    }
}