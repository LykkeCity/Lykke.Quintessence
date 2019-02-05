using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Nethereum.RLP;
using Nethereum.Signer;

namespace Lykke.Quintessence.Core.Blockchain.Strategies
{
    [UsedImplicitly]
    public class BuildEip155TransactionStrategy : IBuildRawTransactionStrategy
    {
        private readonly BigInteger _chainId;
        
        
        public BuildEip155TransactionStrategy(
            IChainId chainId)
        {
            _chainId = chainId.Value;
        }
        
        
        public byte[] Execute(
            byte[][] transactionElements,
            byte[] privateKey)
        {
            transactionElements = transactionElements
                .Append(_chainId.ToBytesForRLPEncoding())
                .Append(0.ToBytesForRLPEncoding())
                .Append(0.ToBytesForRLPEncoding())
                .ToArray();
            
            var signer = new RLPSigner(transactionElements, 6);
            var key = new EthECKey(privateKey, true);
            
            signer.Sign(key, _chainId);

            return signer.GetRLPEncoded();
        }
    }
}