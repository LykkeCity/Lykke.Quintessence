using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Telemetry.ApplicationInsights;

namespace Lykke.Quintessence.Core.Telemetry.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder UseAITelemetryConsumer(
            this ContainerBuilder builder)
        {
            builder
                .RegisterType<AITelemetryConsumer>()
                .As<ITelemetryConsumer>()
                .SingleInstance();

            return builder;
        }
    }
}