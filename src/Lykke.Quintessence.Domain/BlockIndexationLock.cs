using System;
using System.Numerics;

namespace Lykke.Quintessence.Domain
{
    public class BlockIndexationLock
    {
        public BlockIndexationLock(
            BigInteger blockNumber,
            DateTime lockedOn)
        {
            BlockNumber = blockNumber;
            LockedOn = lockedOn;
        }

        
        public BigInteger BlockNumber { get; }
        
        public DateTime LockedOn { get; }
    }
}