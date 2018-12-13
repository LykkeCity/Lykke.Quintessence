using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public interface IApiServicesRegistrant : IServicesRegistrant
    {        
        IReloadingManager<int> GasReserve { get; }
        
        IReloadingManager<string> MaxGasAmount { get; }
    }
}