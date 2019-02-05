using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Lykke.Quintessence.Core.Telemetry.ApplicationInsights
{
    public class AITelemetryConsumer : ITelemetryConsumer
    {
        private readonly TelemetryClient _client;

        
        public AITelemetryConsumer()
        {
            _client = new TelemetryClient();
        }

        public AITelemetryConsumer(
            string instrumentationKey)
        {
            _client = new TelemetryClient
            (
                new TelemetryConfiguration(instrumentationKey)
            );
        }
        
        
        public void TrackDependency(
            string dependencyTypeName,
            string dependencyName,
            string commandName,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success)
        {
            _client.TrackDependency
            (
                dependencyName,
                dependencyName,
                commandName,
                startTime,
                duration,
                success
            );
        }
    }
}