using System;
using System.Numerics;
using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public class DefaultJobServicesRegistrant : IJobServicesRegistrant
    {
        public DefaultJobServicesRegistrant(
            ContainerBuilder builder,
            TimeSpan blockLockDuration,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange,
            bool indexOnlyOwnTransactions,
            BigInteger? maximalBalanceCheckPeriod,
            BigInteger minBlockNumberToIndex)
        {
            BlockLockDuration = blockLockDuration;
            Builder = builder;
            ConfirmationLevel = confirmationLevel;
            GasPriceRange = gasPriceRange;
            IndexOnlyOwnTransactions = indexOnlyOwnTransactions;
            MaximalBalanceCheckPeriod = maximalBalanceCheckPeriod;
            MinBlockNumberToIndex = minBlockNumberToIndex;
        }

        
        public TimeSpan BlockLockDuration { get; }
        
        public ContainerBuilder Builder { get; }
        
        public IReloadingManager<int> ConfirmationLevel { get; }
        
        public IReloadingManager<string> GasPriceRange { get; }
        
        public bool IndexOnlyOwnTransactions { get; }
        
        public BigInteger? MaximalBalanceCheckPeriod { get; }
        
        public BigInteger MinBlockNumberToIndex { get; }
    }
}