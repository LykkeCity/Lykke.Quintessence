using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        internal static ContainerBuilder UseDefaultAddChecksumStrategy(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultHashCalculator();
            
            builder
                .RegisterIfNotRegistered<IAddChecksumStrategy>
                (
                    ctx => new DefaultAddChecksumStrategy
                    (
                        ctx.Resolve<IHashCalculator>()
                    )
                )
                .SingleInstance();
            
            return builder;
        }

        internal static ContainerBuilder UseDefaultDetectContractStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<IDetectContractStrategy, DefaultDetectContractStrategy>();

            return builder;
        }

        internal static ContainerBuilder UseDefaultGetTransactionReceiptsStrategy(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultDetectContractStrategy();
            
            builder
                .RegisterIfNotRegistered<IGetTransactionReceiptsStrategy>
                (
                    ctx => new DefaultGetTransactionReceiptsStrategy
                    (
                        ctx.Resolve<IDetectContractStrategy>(),
                        ctx.Resolve<IEthApiClient>(),
                        ctx.Resolve<IParityApiClient>()
                    )
                )
                .SingleInstance();

            return builder;
        }
        
        internal static ContainerBuilder UseDefaultValidateChecksumStrategy(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultAddChecksumStrategy();
            
            builder
                .RegisterIfNotRegistered<IValidateChecksumStrategy, DefaultValidateChecksumStrategy>()
                .SingleInstance();
            
            return builder;
        }

        internal static ContainerBuilder UseDefaultTryGetTransactionErrorStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<ITryGetTransactionErrorStrategy, DefaultTryGetTransactionErrorStrategy>()
                .SingleInstance();

            return builder;
        }
    }
}