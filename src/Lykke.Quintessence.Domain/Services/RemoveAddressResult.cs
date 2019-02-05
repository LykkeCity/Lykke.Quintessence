namespace Lykke.Quintessence.Domain.Services
{
    public abstract class RemoveAddressResult
    {
        public static NotFoundError NotFound()
            => new NotFoundError();
        
        public static SuccessResult Success()
            => new SuccessResult();
        
        
        public class SuccessResult : RemoveAddressResult
        {
            internal SuccessResult()
            {
                
            }
        }

        public class NotFoundError : RemoveAddressResult
        {
            internal NotFoundError()
            {
                
            }
        }
    }
}
