using System.Numerics;

namespace Lykke.Quintessence.Core.Blockchain
{
    public interface IChainId
    {
        BigInteger Value { get; }
    }
}