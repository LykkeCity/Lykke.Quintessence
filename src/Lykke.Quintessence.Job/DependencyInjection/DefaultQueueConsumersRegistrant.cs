using Autofac;

namespace Lykke.Quintessence.DependencyInjection
{
    public class DefaultQueueConsumersRegistrant : IQueueConsumersRegistrant
    {
        public DefaultQueueConsumersRegistrant(
            ContainerBuilder builder,
            int balanceMonitoringMaxDegreeOfParallelism,
            int blockchainIndexationMaxDegreeOfParallelism,
            int transactionMonitoringMaxDegreeOfParallelism)
        {
            BalanceMonitoringMaxDegreeOfParallelism = balanceMonitoringMaxDegreeOfParallelism;
            BlockchainIndexationMaxDegreeOfParallelism = blockchainIndexationMaxDegreeOfParallelism;
            Builder = builder;
            TransactionMonitoringMaxDegreeOfParallelism = transactionMonitoringMaxDegreeOfParallelism;
        }

        public ContainerBuilder Builder { get; }
        
        public int BalanceMonitoringMaxDegreeOfParallelism { get; }
        
        public int BlockchainIndexationMaxDegreeOfParallelism { get; }
        
        public int TransactionMonitoringMaxDegreeOfParallelism { get; }
    }
}