using System;

namespace Lykke.Quintessence.Core.Telemetry
{
    public interface ITelemetryConsumer
    {
        void TrackDependency(
            string dependencyTypeName,
            string dependencyName,
            string commandName,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success);
    }
}