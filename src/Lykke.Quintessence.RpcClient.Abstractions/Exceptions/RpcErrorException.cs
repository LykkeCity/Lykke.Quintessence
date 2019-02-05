namespace Lykke.Quintessence.RpcClient.Exceptions
{
    public class RpcErrorException : RpcException
    {
        public RpcErrorException(
            string request,
            int errorCode,
            string errorMessage)
        
            : base($"Rpc request failed with error ({errorCode}): {errorMessage}", request)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
        
        
        public int ErrorCode { get; }
        
        public string ErrorMessage { get; }
    }
}
