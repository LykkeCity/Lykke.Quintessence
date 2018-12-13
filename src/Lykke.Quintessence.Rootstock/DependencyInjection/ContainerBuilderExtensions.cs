using Autofac;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Repositories.DependencyInjection;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.DependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder UseRootstock(
            this ContainerBuilder builder,
            IReloadingManager<string> connectionString,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange,
            bool isMainNet)
        {
            var chainId = isMainNet ? 30 : 31;
            
            builder
                .UseChainId(chainId)
                .UseRootstockAddChecksumStrategy();
            
            builder
                .UseAzureRepositories(connectionString)
                .AddRootstockNonceRepository();

            builder
                .RegisterRootstockServices
                (
                    confirmationLevel,
                    gasPriceRange
                )
                .AddRootstockBlockchainService()
                .AddRootstockNonceService();
            
            return builder;
        }
    }
}