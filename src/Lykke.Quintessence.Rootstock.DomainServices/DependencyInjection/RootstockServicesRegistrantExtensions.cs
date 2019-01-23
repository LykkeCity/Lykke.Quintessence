using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class RootstockServicesRegistrantExtensions
    {
        public static IRootstockServicesRegistrant AddRootstockBlockchainService(
            this IRootstockServicesRegistrant registrant)
        {
            registrant
                .AddRootstockNonceService();
            
            registrant
                .Builder
                .UseRootstockDetectContractStrategy()
                .UseRootstockGetTransactionReceiptsStrategy()
                .UseRootstockTryGetTransactionErrorStrategy();
            
            var settings = new DefaultBlockchainService.Settings
            {
                ConfirmationLevel = registrant.ConfirmationLevel,
                GasPriceRange = registrant.GasPriceRange
            };
            
            registrant
                .Builder
                .Register
                (
                    ctx => new RootstockBlockchainService
                    (
                        ctx.Resolve<IApiClient>(),
                        ctx.Resolve<IDetectContractStrategy>(),
                        ctx.Resolve<IGetTransactionReceiptsStrategy>(),
                        ctx.Resolve<ITryGetTransactionErrorStrategy>(),
                        settings
                    )
                )
                .As<IBlockchainService>()
                .SingleInstance();

            return registrant;
        }
        
        public static IRootstockServicesRegistrant AddRootstockNonceService(
            this IRootstockServicesRegistrant registrant)
        {
            registrant
                .Builder
                .Register
                (
                    ctx => new RootstockNonceService
                    (
                        ctx.Resolve<IApiClient>()
                    )
                )
                .As<INonceService>()
                .SingleInstance();

            return registrant;
        }
    }
}