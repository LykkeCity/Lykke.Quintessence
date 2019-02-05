using Autofac;

namespace Lykke.Quintessence.DependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        public static IQueueConsumersRegistrant UseQueueConsumers(
            this ContainerBuilder builder,
            int balanceMonitoringMaxDegreeOfParallelism,
            int blockchainIndexationMaxDegreeOfParallelism,
            int transactionMonitoringMaxDegreeOfParallelism)
        {
            return new DefaultQueueConsumersRegistrant
            (
                builder,
                balanceMonitoringMaxDegreeOfParallelism,
                blockchainIndexationMaxDegreeOfParallelism,
                transactionMonitoringMaxDegreeOfParallelism
            );
        }
    }
}