using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public class DefaultRootstockServicesRegistrant : IRootstockServicesRegistrant
    {
        public DefaultRootstockServicesRegistrant(
            ContainerBuilder builder,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange)
        {
            Builder = builder;
            ConfirmationLevel = confirmationLevel;
            GasPriceRange = gasPriceRange;
        }

        public ContainerBuilder Builder { get; }
        
        public IReloadingManager<int> ConfirmationLevel { get; }
        
        public IReloadingManager<string> GasPriceRange { get; }
    }
}