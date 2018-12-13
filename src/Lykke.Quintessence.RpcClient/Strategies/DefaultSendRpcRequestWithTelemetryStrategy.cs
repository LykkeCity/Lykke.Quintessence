using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Lykke.Quintessence.Core.Telemetry;
using Lykke.Quintessence.RpcClient.Models;
using Newtonsoft.Json;

namespace Lykke.Quintessence.RpcClient.Strategies
{
    public class DefaultSendRpcRequestWithTelemetryStrategy : DefaultSendRpcRequestStrategy
    {
        private readonly string _dependencyName;
        private readonly ITelemetryConsumer _telemetryConsumer;

        
        public DefaultSendRpcRequestWithTelemetryStrategy(
            Uri apiUrl,
            TimeSpan connectionTimeout,
            IHttpClientFactory httpClientFactory,
            ITelemetryConsumer telemetryConsumer) 
            
            : base(apiUrl, connectionTimeout, httpClientFactory)
        {
            _telemetryConsumer = telemetryConsumer;
            _dependencyName = apiUrl.Authority;
        }
        
        public override async Task<string> ExecuteAsync(
            string requestJson)
        {
            var request = JsonConvert.DeserializeObject<RpcRequest>(requestJson);
            var operationStartTime = DateTimeOffset.UtcNow;
            var operationSucceeded = false;
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var response = await base.ExecuteAsync(requestJson);

                operationSucceeded = true;
                    
                return response;
            }
            finally
            {
                _telemetryConsumer.TrackDependency
                (
                    dependencyTypeName: "RpcNode",
                    dependencyName: _dependencyName,
                    commandName: request.Method,
                    startTime: operationStartTime,
                    duration: stopwatch.Elapsed,
                    success: operationSucceeded
                );
            }
        }
    }
}