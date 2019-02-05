using System;
using System.Numerics;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public interface IJobServicesRegistrant : IServicesRegistrant
    {
        TimeSpan BlockLockDuration { get; }
        
        bool IndexOnlyOwnTransactions { get; }
        
        BigInteger? MaximalBalanceCheckPeriod { get; }
        
        BigInteger MinBlockNumberToIndex { get; }
    }
}