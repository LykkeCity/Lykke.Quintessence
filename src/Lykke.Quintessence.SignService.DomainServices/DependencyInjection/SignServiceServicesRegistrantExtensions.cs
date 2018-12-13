using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class SignServiceServicesRegistrantExtensions
    {
        public static ISignServiceServicesRegistrant AddDefaultSignService(
            this ISignServiceServicesRegistrant registrant)
        {
            registrant
                .Builder
                .UseDefaultTransactionBuilder()
                .RegisterIfNotRegistered<ISignService>
                (
                    ctx => new DefaultSignService
                    (
                        ctx.Resolve<ITransactionBuilder>()
                    )
                )
                .SingleInstance();

            return registrant;
        }

        public static ISignServiceServicesRegistrant AddDefaultWalletService(
            this ISignServiceServicesRegistrant registrant)
        {
            registrant
                .Builder
                .UseDefaultAddChecksumStrategy()
                .UseDefaultWalletGenerator()
                .RegisterIfNotRegistered<IWalletService>
                (
                    ctx => new DefaultWalletService
                    (
                        ctx.Resolve<IAddChecksumStrategy>(),
                        ctx.Resolve<IWalletGenerator>()
                    )
                )
                .SingleInstance();
            
            return registrant;
        }
    }
}