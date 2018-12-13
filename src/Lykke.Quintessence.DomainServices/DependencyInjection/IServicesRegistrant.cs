using Autofac;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public interface IServicesRegistrant
    {
        ContainerBuilder Builder { get; }
        
        IReloadingManager<int> ConfirmationLevel { get; }
        
        IReloadingManager<string> GasPriceRange { get; }
    }
}