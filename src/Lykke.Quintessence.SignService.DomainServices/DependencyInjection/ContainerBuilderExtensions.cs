using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static ISignServiceServicesRegistrant RegisterServices(
            this ContainerBuilder builder)
        {
            return new DefaultSignServicesRegistrant
            (
                builder
            );
        }
        
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
    }
}