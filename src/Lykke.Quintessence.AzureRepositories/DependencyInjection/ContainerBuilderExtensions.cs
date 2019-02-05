using Autofac;
using JetBrains.Annotations;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Repositories.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IAzureRepositoriesRegistrant UseAzureRepositories(
            this ContainerBuilder builder,
            IReloadingManager<string> connectionString)
        {
            return new DefaultAzureRepositoriesRegistrant(builder, connectionString);
        }
    }
}