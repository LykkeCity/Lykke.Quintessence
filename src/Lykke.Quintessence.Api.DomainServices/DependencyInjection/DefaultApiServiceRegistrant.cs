using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public class DefaultApiServiceRegistrant : IApiServicesRegistrant
    {
        public DefaultApiServiceRegistrant(
            ContainerBuilder builder,
            IReloadingManager<int> confirmationLevel,
            IReloadingManager<string> gasPriceRange,
            IReloadingManager<int> gasReserve,
            IReloadingManager<string> maxGasAmount)
        {
            Builder = builder;
            ConfirmationLevel = confirmationLevel;
            GasPriceRange = gasPriceRange;
            GasReserve = gasReserve;
            MaxGasAmount = maxGasAmount;
        }

        public ContainerBuilder Builder { get; }
        
        public IReloadingManager<int> ConfirmationLevel { get; }
        
        public IReloadingManager<string> GasPriceRange { get; }
        
        public IReloadingManager<int> GasReserve { get; }
        
        public IReloadingManager<string> MaxGasAmount { get; }
    }
}