using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;
using Lykke.SettingsReader;

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
                        ctx.Resolve<IApiClient>(),
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
    }
}