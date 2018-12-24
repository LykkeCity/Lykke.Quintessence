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
    public static class ApiServicesRegistrantExtensions
    {
        public static IApiServicesRegistrant AddDefaultBalanceService(
            this IApiServicesRegistrant registrant)
        {
            registrant
                .AddDefaultBlockchainService();
            
            registrant
                .Builder
                .RegisterIfNotRegistered<IBalanceService>
                (
                    ctx => new DefaultBalanceService
                    (
                        ctx.Resolve<IBalanceMonitoringTaskRepository>(),
                        ctx.Resolve<IBalanceRepository>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<ILogFactory>()
                    )
                )
                .SingleInstance();

            return registrant;
        }
        
        public static IApiServicesRegistrant AddDefaultTransactionHistoryService(
            this IApiServicesRegistrant registrant)
        {
            registrant
                .Builder
                .RegisterIfNotRegistered<ITransactionHistoryService>
                (
                    ctx => new DefaultTransactionHistoryService
                    (
                        ctx.Resolve<ITransactionHistoryObservationAddressesRepository>(),
                        ctx.Resolve<ITransactionReceiptRepository>()
                    )
                )
                .SingleInstance();

            return registrant;
        }
        
        public static IApiServicesRegistrant AddDefaultTransactionService(
            this IApiServicesRegistrant registrant)
        {
            registrant
                .Builder
                .UseDefaultBuildTransactionStrategy()
                .UseDefaultCalculateGasAmountStrategy(registrant.GasReserve, registrant.MaxGasAmount)
                .UseDefaultCalculateTransactionAmountStrategy();
            
            registrant
                .AddDefaultAddressService()
                .AddDefaultBlockchainService();
            
            registrant
                .Builder
                .RegisterIfNotRegistered<ITransactionService>
                (
                    ctx => new DefaultTransactionService
                    (
                        ctx.Resolve<IAddressService>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<IChaosKitty>(),
                        ctx.Resolve<ILogFactory>(),
                        ctx.Resolve<ITransactionMonitoringTaskRepository>(),
                        ctx.Resolve<ITransactionRepository>(),
                        ctx.Resolve<IBuildTransactionStrategy>(),
                        ctx.Resolve<ICalculateGasAmountStrategy>(),
                        ctx.Resolve<ICalculateTransactionAmountStrategy>()
                    )
                )
                .SingleInstance();

            return registrant;
        }
    }
}