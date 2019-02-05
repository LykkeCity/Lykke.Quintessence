using System.ComponentModel;
using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Models
{
    [PublicAPI, ImmutableObject(true)]
    public sealed class TransactionReceipt
    {
        public TransactionReceipt(
            string blockHash,
            BigInteger? blockNumber,
            string contractAddress,
            BigInteger cumulativeGasUsed,
            BigInteger gasUsed,
            BigInteger? status,
            BigInteger transactionIndex,
            string transactionHash)
        {
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            ContractAddress = contractAddress;
            CumulativeGasUsed = cumulativeGasUsed;
            GasUsed = gasUsed;
            Status = status;
            TransactionIndex = transactionIndex;
            TransactionHash = transactionHash;
        }
        
        /// <summary>
        ///    Hash of the block where this transaction was included.
        /// </summary>
        public string BlockHash { get; }

        /// <summary>
        ///    Block number where this transaction was included.
        /// </summary>
        public BigInteger? BlockNumber { get; }

        /// <summary>
        ///    The contract address created, if the transaction was a contract creation, otherwise null.
        /// </summary>
        public string ContractAddress { get; }

        /// <summary>
        ///    The total amount of gas used when this transaction was executed in the block.
        /// </summary>
        public BigInteger CumulativeGasUsed { get; }

        /// <summary>
        ///    The amount of gas used by this specific transaction alone.
        /// </summary>
        public BigInteger GasUsed { get; }

        /// <summary>
        ///    Transaction Success 1, Transaction Failed 0, null is status is not supported
        /// </summary>
        public BigInteger? Status { get; }

        /// <summary>
        ///    Integer of the transactions index position in the block.
        /// </summary>
        public BigInteger TransactionIndex { get; }

        /// <summary>
        ///    Hash of the transaction.
        /// </summary>
        public string TransactionHash { get; }
    }
}
