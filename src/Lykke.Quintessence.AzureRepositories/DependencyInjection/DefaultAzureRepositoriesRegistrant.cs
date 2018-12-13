using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories.DependencyInjection
{
    public class DefaultAzureRepositoriesRegistrant : IAzureRepositoriesRegistrant
    {
        internal DefaultAzureRepositoriesRegistrant(
            ContainerBuilder builder,
            IReloadingManager<string> connectionString)
        {
            Builder = builder;
            ConnectionString = connectionString;
        }
        
        public ContainerBuilder Builder { get; }
            
        public IReloadingManager<string> ConnectionString { get; }
    }
}