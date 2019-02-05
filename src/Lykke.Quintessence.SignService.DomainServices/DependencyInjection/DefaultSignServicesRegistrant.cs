using Autofac;

namespace Lykke.Quintessence.Domain.Services.DependencyInjection
{
    public class DefaultSignServicesRegistrant : ISignServiceServicesRegistrant
    {
        public DefaultSignServicesRegistrant(
            ContainerBuilder builder)
        {
            Builder = builder;
        }

        public ContainerBuilder Builder { get; }
    }
}