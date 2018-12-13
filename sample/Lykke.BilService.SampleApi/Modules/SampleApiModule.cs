using Autofac;
using JetBrains.Annotations;
using Lykke.BilService.SampleApi.DomainServices;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Domain.Services.DependencyInjection;

namespace Lykke.BilService.SampleApi.Modules
{
    [UsedImplicitly]
    public class SampleApiModule : Module
    {
        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .UseChainId(0)
                .UseAssetService<SampleAssetService>();
        }
    }
}