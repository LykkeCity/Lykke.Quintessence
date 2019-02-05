using Autofac;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public interface ISignServiceServicesRegistrant
    {
        ContainerBuilder Builder { get; }
        
    }
}