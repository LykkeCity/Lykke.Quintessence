using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IApiServicesRegistrant RegisterServices(
            this ContainerBuilder builder,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange,
            IReloadingManager<int> gasReserve,
            IReloadingManager<string> maxGasAmount)
        {
            return new DefaultApiServiceRegistrant
            (
                builder,
                confirmationLevel,
                gasPriceRange,
                gasReserve,
                maxGasAmount
            );
        }

        public static ContainerBuilder UseAssetService<T>(
            this ContainerBuilder builder)
        
            where T : IAssetService
        {
            builder
                .RegisterType<T>()
                .As<IAssetService>()
                .SingleInstance();

            return builder;
        }

        internal static ContainerBuilder UseDefaultBuildTransactionStrategy(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultCalculateGasPriceStrategy()
                .RegisterIfNotRegistered<IBuildTransactionStrategy>
                (
                    ctx => new DefaultBuildTransactionStrategy
                    (
                        ctx.Resolve<ICalculateGasPriceStrategy>()
                    )
                )
                .SingleInstance();

            return builder;
        }
        
        internal static ContainerBuilder UseDefaultCalculateGasAmountStrategy(
            this ContainerBuilder builder,
            IReloadingManager<int>  gasReserve,
            IReloadingManager<string>  maxGasAmount)
        {
            builder
                .UseDefaultCalculateGasPriceStrategy()
                .RegisterIfNotRegistered<ICalculateGasAmountStrategy>
                (
                    ctx => new DefaultCalculateGasAmountStrategy
                    (
                        gasReserve,
                        maxGasAmount
                    )
                )
                .SingleInstance();

            return builder;
        }
        
        internal static ContainerBuilder UseDefaultCalculateGasPriceStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<ICalculateGasPriceStrategy>
                (
                    ctx => new DefaultCalculateGasPriceStrategy()
                )
                .SingleInstance();

            return builder;
        }
        
        internal static ContainerBuilder UseDefaultCalculateTransactionAmountStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<ICalculateTransactionAmountStrategy>
                (
                    ctx => new DefaultCalculateTransactionAmountStrategy()
                )
                .SingleInstance();

            return builder;
        }
    }
}