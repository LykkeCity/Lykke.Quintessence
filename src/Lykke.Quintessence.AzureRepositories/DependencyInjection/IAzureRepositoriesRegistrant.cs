using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories.DependencyInjection
{
    public interface IAzureRepositoriesRegistrant
    {
        ContainerBuilder Builder { get; }
            
        IReloadingManager<string> ConnectionString { get; }
    }
}