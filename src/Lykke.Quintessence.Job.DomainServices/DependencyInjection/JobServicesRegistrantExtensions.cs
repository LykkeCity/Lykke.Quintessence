using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    [PublicAPI]
    public static class JobServicesRegistrantExtensions
    {
        public static IJobServicesRegistrant AddDefaultBalanceCheckSchedulingService(
            this IJobServicesRegistrant registrant)
        {
            if (registrant.MaximalBalanceCheckPeriod.HasValue)
            {
                var settings = new DefaultBalanceCheckSchedulingService.Settings
                {
                    MaximalBalanceCheckPeriod = registrant.MaximalBalanceCheckPeriod.Value
                };
            
                registrant
                    .Builder
                    .RegisterIfNotRegistered<IBalanceCheckSchedulingService>
                    (
                        ctx => new DefaultBalanceCheckSchedulingService
                        (
                            ctx.Resolve<IBalanceCheckSchedulerLockRepository>(),
                            ctx.Resolve<IBalanceRepository>(),
                            ctx.Resolve<IBalanceMonitoringTaskRepository>(),
                            ctx.Resolve<IBlockchainService>(),
                            ctx.Resolve<ILogFactory>(),
                            settings
                        )
                    )
                    .SingleInstance();
            }
            
            return registrant;
        }
        
        public static IJobServicesRegistrant AddDefaultBalanceMonitoringService(
            this IJobServicesRegistrant registrant)
        {
            registrant
                .AddDefaultBlockchainService();
            
            registrant
                .Builder
                .RegisterIfNotRegistered<IBalanceMonitoringService>
                (
                    ctx => new DefaultBalanceMonitoringService
                    (
                        ctx.Resolve<IBalanceMonitoringTaskRepository>(),
                        ctx.Resolve<IBalanceRepository>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<IChaosKitty>(),
                        ctx.Resolve<ILogFactory>()
                    )
                )
                .SingleInstance();
            
            return registrant;
        }
        
        public static IJobServicesRegistrant AddDefaultBlockchainIndexingService(
            this IJobServicesRegistrant registrant)
        {
            registrant
                .Builder
                .UseDefaultIndexBlockStrategy(registrant.IndexOnlyOwnTransactions);
            
            registrant
                .AddDefaultBlockchainService();
            
            var settings = new DefaultBlockchainIndexingService.Settings
            {
                BlockLockDuration = registrant.BlockLockDuration,
                MinBlockNumberToIndex = registrant.MinBlockNumberToIndex
            };
                
            registrant
                .Builder
                .RegisterIfNotRegistered<IBlockchainIndexingService>
                (
                    ctx => new DefaultBlockchainIndexingService
                    (
                        ctx.Resolve<IBalanceMonitoringTaskRepository>(),
                        ctx.Resolve<IBalanceRepository>(),
                        ctx.Resolve<IBlockchainIndexationStateRepository>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<IBlockIndexationLockRepository>(),
                        ctx.Resolve<IChaosKitty>(),
                        ctx.Resolve<IIndexBlockStrategy>(),
                        ctx.Resolve<ILogFactory>(),
                        settings,
                        ctx.Resolve<ITransactionReceiptRepository>()
                    )
                )
                .SingleInstance();
            
            return registrant;
        }

        public static IJobServicesRegistrant AddDefaultTransactionMonitoringService(
            this IJobServicesRegistrant registrant)
        {
            registrant
                .AddDefaultBlockchainService()
                .AddDefaultAddressService();
            
            registrant
                .Builder
                .RegisterIfNotRegistered<ITransactionMonitoringService>
                (
                    ctx => new DefaultTransactionMonitoringService
                    (
                        ctx.Resolve<IAddressService>(),
                        ctx.Resolve<IBlockchainService>(),
                        ctx.Resolve<ILogFactory>(),
                        ctx.Resolve<ITransactionMonitoringTaskRepository>(),
                        ctx.Resolve<ITransactionRepository>()
                    )
                )
                .SingleInstance();

            return registrant;
        }
    }
}