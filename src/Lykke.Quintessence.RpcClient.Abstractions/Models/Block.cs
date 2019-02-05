using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.Models
{
    [PublicAPI, ImmutableObject(true)]
    public sealed class Block
    {
        public Block(
            string blockHash,
            BigInteger? number,
            string parentHash,
            BigInteger timestamp,
            IEnumerable<string> transactionHashes,
            IEnumerable<Transaction> transactions)
        {
            BlockHash = blockHash;
            Number = number;
            ParentHash = parentHash;
            Timestamp = timestamp;
            TransactionHashes = transactionHashes.ToImmutableArray();
            Transactions = transactions?.ToImmutableArray();
        }

        /// <summary>
        ///    Hash of the block.
        /// </summary>
        public string BlockHash { get; }
        
        /// <summary>
        ///    The block number. Null if the block is pending.
        /// </summary>
        public BigInteger? Number { get; }

        /// <summary>
        ///    Hash of the parent block.
        /// </summary>
        public string ParentHash { get; }

        /// <summary>
        ///    The unix timestamp (in seconds) for when the block was collated.
        /// </summary>
        public BigInteger Timestamp { get; }

        /// <summary>
        ///    List of transaction hashes.
        /// </summary>
        public ImmutableArray<string> TransactionHashes { get; }

        /// <summary>
        ///    List of transactions.
        /// </summary>
        public ImmutableArray<Transaction>? Transactions { get; }
    }
}
