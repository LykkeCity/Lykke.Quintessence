using System;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Exceptions
{
    [PublicAPI]
    public class RpcException : Exception
    {
        public RpcException(
            string message,
            string request,
            Exception inner = null)
        
            : base(message, inner)
        {
            Request = request;
        }
        
        public string Request { get; }
    }
}
