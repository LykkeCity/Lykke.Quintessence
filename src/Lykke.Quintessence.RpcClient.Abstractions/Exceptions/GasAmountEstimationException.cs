using System;
using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Exceptions
{
    [PublicAPI]
    public class GasAmountEstimationException : Exception
    {
        public GasAmountEstimationException(
            string from,
            string to,
            BigInteger transferAmount,
            Exception innerException = null)
        
            : base("Gas amount can not be estimated.", innerException)
        {
            From = from;
            To = to;
            TransferAmount = transferAmount;
        }
        
        
        public string From { get; }
        
        public string To { get; }
        
        public BigInteger TransferAmount { get; }
    }
}