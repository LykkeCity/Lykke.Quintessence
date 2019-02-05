using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class BlockIntervalIndexationState
    {
        public BlockIntervalIndexationState(
            BigInteger from,
            bool isIndexed,
            BigInteger to)
        {
            From = from;
            IsIndexed = isIndexed;
            To = to;
        }

        
        public BigInteger From { get; }
        
        public bool IsIndexed { get; }
        
        public BigInteger To { get; }
    }
}