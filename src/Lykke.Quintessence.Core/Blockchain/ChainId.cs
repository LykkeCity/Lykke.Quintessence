using System.Numerics;

namespace Lykke.Quintessence.Core.Blockchain
{
    public class ChainId : IChainId
    {
        public ChainId(
            BigInteger value)
        {
            Value = value;
        }

        public BigInteger Value { get; }
    }
}