using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class ServicesRegistrantExtensions
    {
        public static T AddDefaultAddressService<T>(
            this T registrant)

            where T : IServicesRegistrant
        {
            registrant
                .Builder
                .UseDefaultAddChecksumStrategy()
                .UseDefaultValidateChecksumStrategy();

            registrant
                .AddDefaultBlockchainService();
            
            registrant
                .Builder
                .RegisterIfNotRegistered<IAddressService>
                (
                    ctx => new DefaultAddressService
                    (
                        ctx.Resolve<IAddChecksumStrategy>(),
                        ctx.Resolve<IBlacklistedAddressRepository>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<ILogFactory>(),
                        ctx.Resolve<IValidateChecksumStrategy>(),
                        ctx.Resolve<IWhitelistedAddressRepository>()
                    )
                )
                .SingleInstance();
            
            return registrant;
        }

        public static T AddDefaultBlockchainService<T>(
            this T registrant)

            where T : IServicesRegistrant
        {
            registrant
                .Builder
                .UseDefaultDetectContractStrategy()
                .UseDefaultGetTransactionReceiptsStrategy()
                .UseDefaultTryGetTransactionErrorStrategy();

            registrant
                .AddDefaultNonceService();
            
            var settings = new DefaultBlockchainService.Settings
            {
                ConfirmationLevel = registrant.ConfirmationLevel,
                GasPriceRange = registrant.GasPriceRange
            };
            
            registrant
                .Builder
                .RegisterIfNotRegistered<IBlockchainService>
                (
                    ctx => new DefaultBlockchainService
                    (
                        ctx.Resolve<IDetectContractStrategy>(),
                        ctx.Resolve<IApiClient>(),
                        ctx.Resolve<IGetTransactionReceiptsStrategy>(),
                        ctx.Resolve<INonceService>(),
                        ctx.Resolve<ITryGetTransactionErrorStrategy>(),
                        settings   
                    )
                )
                .SingleInstance();
            
            return registrant;
        }

        public static T AddDefaultNonceService<T>(
            this T registrant)
        
            where T : IServicesRegistrant
        {
            registrant
                .Builder
                .RegisterIfNotRegistered<INonceService>
                (
                    ctx => new DefaultNonceService
                    (
                        ctx.Resolve<IParityApiClient>()
                    )
                )
                .SingleInstance();

            return registrant;
        }
    }
}