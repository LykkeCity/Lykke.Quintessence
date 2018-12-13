using System.Collections.Generic;
using System.Collections.Immutable;
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
            TransactionResult result,
            int subTraces,
            IEnumerable<int> traceAddresses,
            string transactionHash,
            int transactionPosition,
            string type)
        {
            Action = action;
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            Error = error;
            Result = result;
            SubTraces = subTraces;
            TraceAddresses = traceAddresses.ToImmutableArray();
            TransactionHash = transactionHash;
            TransactionPosition = transactionPosition;
            Type = type;
        }

        
        public TransactionAction Action { get; }

        public string BlockHash { get; }

        public BigInteger BlockNumber { get; }

        public string Error { get; }

        public TransactionResult Result { get; }

        public int SubTraces { get; }

        public ImmutableArray<int> TraceAddresses { get; }

        public string TransactionHash { get; }

        public int TransactionPosition { get; }

        public string Type { get; }

        
        [PublicAPI]
        public class TransactionAction
        {
            public TransactionAction(
                string callType,
                string from,
                BigInteger gas,
                string to,
                BigInteger value)
            {
                CallType = callType;
                From = from;
                Gas = gas;
                To = to;
                Value = value;
            }

            public string CallType { get; }

            public string From { get; }

            public BigInteger Gas { get; }
            
            public string To { get; }

            public BigInteger Value { get; }
        }

        [PublicAPI]
        public class TransactionResult
        {
            public TransactionResult(
                string address,
                BigInteger gasUsed, 
                string output)
            {
                GasUsed = gasUsed;
                Output = output;
                Address = address;
            }

            public string Address { get; }
            
            public BigInteger GasUsed { get; }

            public string Output { get; }
        }
    }
}