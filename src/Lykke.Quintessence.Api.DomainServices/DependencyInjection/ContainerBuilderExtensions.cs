using Autofac;
using JetBrains.Annotations;
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
                .RegisterIfNotRegistered<IBuildTransactionStrategy>
                (
                    ctx => new DefaultBuildTransactionStrategy
                    (
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<INonceService>(),
                        ctx.Resolve<ITransactionRepository>()
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