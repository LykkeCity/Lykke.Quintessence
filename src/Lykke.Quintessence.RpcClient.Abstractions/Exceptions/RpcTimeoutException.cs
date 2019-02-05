using System;

namespace Lykke.Quintessence.RpcClient.Exceptions
{
    public class RpcTimeoutException : RpcException
    {
        public RpcTimeoutException(
            TimeSpan connectionTimeout,
            string request) 
            
            : base($"RPC request timed out after {connectionTimeout.TotalMilliseconds} milliseconds.", request)
        {
            
        }
    }
}
