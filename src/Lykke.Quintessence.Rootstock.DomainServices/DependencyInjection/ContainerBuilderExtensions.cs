using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IRootstockServicesRegistrant RegisterRootstockServices(
            this ContainerBuilder builder,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange)
        {
            return new DefaultRootstockServicesRegistrant
            (
                builder,
                confirmationLevel,
                gasPriceRange
            );
        }
        
        public static ContainerBuilder UseRootstockAddChecksumStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterType<RootstockAddChecksumStrategy>()
                .As<IAddChecksumStrategy>()
                .SingleInstance();

            return builder;
        }

        internal static ContainerBuilder UseRootstockDetectContractStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterType<RootstockDetectContractStrategy>()
                .As<IDetectContractStrategy>()
                .SingleInstance();

            return builder;
        }

        internal static ContainerBuilder UseRootstockGetTransactionReceiptsStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterType<RootstockGetTransactionReceiptsStrategy>()
                .As<IGetTransactionReceiptsStrategy>()
                .SingleInstance();

            return builder;
        }

        internal static ContainerBuilder UseRootstockTryGetTransactionErrorStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterType<RootstockTryGetTransactionErrorStrategy>()
                .As<ITryGetTransactionErrorStrategy>()
                .SingleInstance();

            return builder;
        }
    }
}