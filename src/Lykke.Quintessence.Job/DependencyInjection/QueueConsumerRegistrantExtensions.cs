using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.QueueConsumers;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.QueueConsumers;

namespace Lykke.Quintessence.DependencyInjection
{
    [PublicAPI]
    public static class QueueConsumerRegistrantExtensions
    {
        public static IQueueConsumersRegistrant AddBalanceMonitoringQueueConsumer(
            this IQueueConsumersRegistrant registrant)
        {
            registrant
                .Builder
                .Register
                (
                    ctx => new DefaultBalanceMonitoringQueueConsumer
                    (
                        ctx.Resolve<IBalanceMonitoringService>(),
                        ctx.Resolve<ILogFactory>(),
                        registrant.BalanceMonitoringMaxDegreeOfParallelism
                    )
                )
                .As<IBalanceMonitoringQueueConsumer>()
                .As<IStartable>()
                .As<IStopable>()
                .IfNotRegistered(typeof(IBalanceMonitoringQueueConsumer))
                .SingleInstance();

            return registrant;
        }
        
        public static IQueueConsumersRegistrant AddBlockchainIndexationQueueConsumer(
            this IQueueConsumersRegistrant registrant)
        {
            registrant
                .Builder
                .Register
                (
                    ctx => new DefaultBlockchainIndexationQueueConsumer
                    (
                        ctx.Resolve<IBlockchainIndexingService>(),
                        ctx.Resolve<ILogFactory>(),
                        registrant.BlockchainIndexationMaxDegreeOfParallelism
                    )
                )
                .As<IBlockchainIndexationQueueConsumer>()
                .As<IStartable>()
                .As<IStopable>()
                .IfNotRegistered(typeof(IBlockchainIndexationQueueConsumer))
                .SingleInstance();

            return registrant;
        }
        
        public static IQueueConsumersRegistrant AddTransactionMonitoringQueueConsumer(
            this IQueueConsumersRegistrant registrant)
        {
            registrant
                .Builder
                .Register
                (
                    ctx => new DefaultTransactionMonitoringQueueConsumer
                    (
                        ctx.Resolve<ILogFactory>(),
                        ctx.Resolve<ITransactionMonitoringService>(),
                        registrant.TransactionMonitoringMaxDegreeOfParallelism
                    )
                )
                .As<ITransactionMonitoringQueueConsumer>()
                .As<IStartable>()
                .As<IStopable>()
                .IfNotRegistered(typeof(ITransactionMonitoringQueueConsumer))
                .SingleInstance();

            return registrant;
        }
    }
}