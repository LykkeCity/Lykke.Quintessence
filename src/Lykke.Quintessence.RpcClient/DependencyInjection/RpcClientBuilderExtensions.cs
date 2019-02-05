using Autofac;
using JetBrains.Annotations;

namespace Lykke.Quintessence.RpcClient.DependencyInjection
{
    [PublicAPI]
    public static class RpcClientBuilderExtensions
    {
        public static IRpcClientBuilder AddDefaultEthApi(
            this IRpcClientBuilder builder)
        {
            builder
                .UseDefaultSendRpcRequestStrategy();

            builder
                .Builder
                .RegisterType<DefaultEthApiClient>()
                .As<IEthApiClient>()
                .SingleInstance()
                .IfNotRegistered(typeof(IEthApiClient));

            return builder;
        }
        
        public static IRpcClientBuilder AddDefaultParityApi(
            this IRpcClientBuilder builder)
        {
            builder
                .UseDefaultSendRpcRequestStrategy();

            builder
                .Builder
                .RegisterType<DefaultParityApiClient>()
                .As<IParityApiClient>()
                .SingleInstance()
                .IfNotRegistered(typeof(IParityApiClient));

            return builder;
        }

        private static IRpcClientBuilder UseDefaultSendRpcRequestStrategy(
            this IRpcClientBuilder builder)
        {
            builder
                .Builder
                .UseDefaultSendRpcRequestStrategy
                (
                    builder.ApiUrl,
                    builder.ConnectionTimeout,
                    builder.EnableTelemetry
                );

            return builder;
        }
    }
}