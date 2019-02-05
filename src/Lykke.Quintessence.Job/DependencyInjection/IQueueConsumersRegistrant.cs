using Autofac;

namespace Lykke.Quintessence.DependencyInjection
{
    public interface IQueueConsumersRegistrant
    {
        ContainerBuilder Builder { get; }
        
        int BalanceMonitoringMaxDegreeOfParallelism { get; }
        
        int BlockchainIndexationMaxDegreeOfParallelism { get; }
        
        int TransactionMonitoringMaxDegreeOfParallelism { get; }
    }
}