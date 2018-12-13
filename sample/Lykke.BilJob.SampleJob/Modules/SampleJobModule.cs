using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Telemetry.DependencyInjection;

namespace Lykke.BilJob.SampleJob.Modules
{
    [UsedImplicitly]
    public class SampleJobModule : Module
    {
        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .UseAITelemetryConsumer();
            
            builder
                .RegisterInstance(new ChainId(0))
                .As<IChainId>();
        }
    }
}