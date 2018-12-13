using System;
using Autofac;

namespace Lykke.Quintessence.RpcClient.DependencyInjection
{
    public class DefaultRpcClientBuilder : IRpcClientBuilder
    {
        internal DefaultRpcClientBuilder(
            Uri apiUrl,
            ContainerBuilder builder,
            TimeSpan connectionTimeout,
            bool enableTelemetry)
        {
            ApiUrl = apiUrl;
            Builder = builder;
            ConnectionTimeout = connectionTimeout;
            EnableTelemetry = enableTelemetry;
        }

        public Uri ApiUrl { get; }
            
        public ContainerBuilder Builder { get; }
            
        public TimeSpan ConnectionTimeout { get; }
            
        public bool EnableTelemetry { get; }
    }
}