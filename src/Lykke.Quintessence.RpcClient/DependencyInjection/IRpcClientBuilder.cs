using System;
using Autofac;

namespace Lykke.Quintessence.RpcClient.DependencyInjection
{
    public interface IRpcClientBuilder
    {
        Uri ApiUrl { get; }
            
        ContainerBuilder Builder { get; }
            
        TimeSpan ConnectionTimeout { get; }
            
        bool EnableTelemetry { get; }
    }
}