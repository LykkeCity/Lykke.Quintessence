using Autofac;
using JetBrains.Annotations;
using Lykke.BilService.SampleApi.DomainServices;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Core.Telemetry.DependencyInjection;
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
                .UseAITelemetryConsumer()
                .UseChainId(3)
                .UseAssetService<SampleAssetService>();
        }
    }
}