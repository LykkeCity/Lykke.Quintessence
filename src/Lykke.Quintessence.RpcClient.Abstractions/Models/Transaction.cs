using System.ComponentModel;
using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Models
{
    [PublicAPI, ImmutableObject(true)]
    public sealed class Transaction
    {
        public Transaction(
            string blockHash,
            BigInteger? blockNumber,
            string from,
            BigInteger gas,
            BigInteger gasPrice,
            string input,
            BigInteger nonce,
            string to,
            BigInteger? transactionIndex,
            string transactionHash,
            BigInteger value)
        {
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            From = from;
            Gas = gas;
            GasPrice = gasPrice;
            Input = input;
            Nonce = nonce;
            To = to;
            TransactionIndex = transactionIndex;
            TransactionHash = transactionHash;
            Value = value;
        }

        /// <summary>
        ///    Hash of the block where this transaction was included in. Null when it is pending.
        /// </summary>
        public string BlockHash { get; }

        /// <summary>
        ///    Block number where this transaction was included in. Null when it is pending.
        /// </summary>
        public BigInteger? BlockNumber { get; }

        /// <summary>
        ///    The address the transaction is sent from.
        /// </summary>
        [NotNull]
        public string From { get; }

        /// <summary>
        ///    Amount of gas provided by the sender.
        /// </summary>
        public BigInteger Gas { get; }

        /// <summary>
        ///   Gas price provided by the sender (wei).
        /// </summary>
        public BigInteger GasPrice { get; }

        /// <summary>
        ///    The data sent alongside with the transaction.
        /// </summary>
        public string Input { get; }

        /// <summary>
        ///    The number of transactions made by the sender prior to this one.
        /// </summary>
        public BigInteger Nonce { get; }

        /// <summary>
        ///    The address the transaction is sent to. Null when it is a contract creation transaction.
        /// </summary>
        public string To { get; }

        /// <summary>
        ///    Integer of the transactions index position in the block. Null when it is pending.
        /// </summary>
        public BigInteger? TransactionIndex { get; }

        /// <summary>
        ///    Hash of the transaction.
        /// </summary>
        public string TransactionHash { get; }

        /// <summary>
        ///    Transferred value (wei).
        /// </summary>
        public BigInteger Value { get; }
    }
}
