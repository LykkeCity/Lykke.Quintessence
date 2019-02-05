using System.ComponentModel;
using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Models
{
    [PublicAPI, ImmutableObject(true)]
    public sealed class TransactionTrace
    {
        public TransactionTrace(
            TransactionAction action,
            string blockHash,
            BigInteger blockNumber,
            string error,
            string transactionHash,
            string type)
        {
            Action = action;
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            Error = error;
            TransactionHash = transactionHash;
            Type = type;
        }

        
        public TransactionAction Action { get; }

        public string BlockHash { get; }

        public BigInteger BlockNumber { get; }

        public string Error { get; }

        public string TransactionHash { get; }

        public string Type { get; }

        
        [PublicAPI]
        public class TransactionAction
        {
            public TransactionAction(
                string callType,
                string from,
                string to,
                BigInteger value)
            {
                CallType = callType;
                From = from;
                To = to;
                Value = value;
            }

            public string CallType { get; }

            public string From { get; }

            public string To { get; }

            public BigInteger Value { get; }
        }
    }
}