using System.Numerics;
using JetBrains.Annotations;
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
            var signer = new RLPSigner(transactionElements);
            var key = new EthECKey(privateKey, true);
            
            signer.Sign(key, _chainId);

            return signer.GetRLPEncoded();
        }
    }
}