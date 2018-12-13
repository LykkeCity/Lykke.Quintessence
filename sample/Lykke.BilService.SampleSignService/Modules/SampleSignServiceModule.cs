using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.DependencyInjection;

namespace Lykke.BilService.SampleSignService.Modules
{
    [UsedImplicitly]
    public class SampleSignServiceModule : Module
    {
        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .UseChainId(0);
        }
    }
}