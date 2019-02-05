using System;
using System.Numerics;
using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IJobServicesRegistrant RegisterServices(
            this ContainerBuilder builder,
            TimeSpan blockLockDuration,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange,
            bool indexOnlyOwnTransactions,
            BigInteger? maximalBalanceCheckPeriod, 
            BigInteger minBlockNumberToIndex)
        {
            return new DefaultJobServicesRegistrant
            (
                builder,
                blockLockDuration,
                confirmationLevel,
                gasPriceRange,
                indexOnlyOwnTransactions,
                maximalBalanceCheckPeriod,
                minBlockNumberToIndex
            );
        }
        
        internal static ContainerBuilder UseDefaultIndexBlockStrategy(
            this ContainerBuilder builder,
            bool indexOnlyOwnTransactions)
        {
            builder
                .RegisterIfNotRegistered<IIndexBlockStrategy, DefaultIndexBlockStrategy>
                (
                    ctx => new DefaultIndexBlockStrategy(indexOnlyOwnTransactions)
                )
                .SingleInstance();

            return builder;
        }
    }
}