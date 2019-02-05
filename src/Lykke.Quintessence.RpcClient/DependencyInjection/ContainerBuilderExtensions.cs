using System;
using System.Net.Http;
using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Telemetry;
using Lykke.Quintessence.RpcClient.Strategies;

namespace Lykke.Quintessence.RpcClient.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static IRpcClientBuilder UseRpcClient(
            this ContainerBuilder builder,
            string apiUrl,
            int connectionTimeout,
            bool enableTelemetry)
        {
            return new DefaultRpcClientBuilder
            (
                new Uri(apiUrl, UriKind.Absolute),
                builder,
                TimeSpan.FromSeconds(connectionTimeout),
                enableTelemetry
            );
        }
        
        internal static ContainerBuilder UseDefaultSendRpcRequestStrategy(
            this ContainerBuilder builder,
            Uri apiUrl,
            TimeSpan connectionTimeout,
            bool enableTelemetry)
        {
            builder
                .Register(ctx =>
                {
                    if (enableTelemetry)
                    {
                        return new DefaultSendRpcRequestWithTelemetryStrategy
                        (
                            apiUrl,
                            connectionTimeout,
                            ctx.Resolve<IHttpClientFactory>(),
                            ctx.Resolve<ITelemetryConsumer>()
                        );
                    }
                    else
                    {
                        return new DefaultSendRpcRequestStrategy
                        (
                            apiUrl,
                            connectionTimeout,
                            ctx.Resolve<IHttpClientFactory>()
                        );
                    }
                })
                .As<ISendRpcRequestStrategy>()
                .IfNotRegistered(typeof(ISendRpcRequestStrategy));

            return builder;
        }
    }
}