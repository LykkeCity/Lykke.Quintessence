using System;
using System.Numerics;
using Autofac;
using Autofac.Builder;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Blockchain.Strategies;
using Lykke.Quintessence.Core.Crypto;

namespace Lykke.Quintessence.Core.DependencyInjection
{
    [PublicAPI]
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder UseChainId(
            this ContainerBuilder builder,
            BigInteger chainId)
        {
            builder
                .RegisterInstance(new ChainId(chainId))
                .As<IChainId>();

            return builder;
        }

        public static ContainerBuilder UseDefaultBuildRawTransactionStrategy(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<IBuildRawTransactionStrategy, BuildEip155TransactionStrategy>()
                .SingleInstance();

            return builder;
        }
        
        public static ContainerBuilder UseDefaultHashCalculator(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<IHashCalculator, HashCalculator>(HashCalculator.Keccak256)
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder UseDefaultKeyGenerator(
            this ContainerBuilder builder)
        {
            builder
                .RegisterIfNotRegistered<IKeyGenerator, DefaultKeyGenerator>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder UseDefaultTransactionBuilder(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultBuildRawTransactionStrategy();

            builder
                .RegisterIfNotRegistered<ITransactionBuilder, DefaultTransactionBuilder>()
                .SingleInstance();
            
            return builder;
        }

        public static ContainerBuilder UseDefaultWalletGenerator(
            this ContainerBuilder builder)
        {
            builder
                .UseDefaultHashCalculator()
                .UseDefaultKeyGenerator();
            
            builder
                .RegisterIfNotRegistered<IWalletGenerator, DefaultWalletGenerator>()
                .SingleInstance();
            
            return builder;
        }

        public static IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterIfNotRegistered<TService, TImplementation>(
            this ContainerBuilder builder)
        
            where TImplementation : TService
        {
            return builder
                .RegisterType<TImplementation>()
                .As<TService>()
                .IfNotRegistered(typeof(TService));
        }
        
        public static IRegistrationBuilder<TImplementation, SimpleActivatorData, SingleRegistrationStyle> RegisterIfNotRegistered<TService, TImplementation>(
            this ContainerBuilder builder,
            TImplementation instance)
        
            where TImplementation : class, TService
        {
            return builder
                .RegisterInstance(instance)
                .As<TService>()
                .IfNotRegistered(typeof(TService));
        }
        
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterIfNotRegistered<TService>(
            this ContainerBuilder builder,
            Func<IComponentContext, TService> @delegate)
        {
            return builder
                .Register(@delegate)
                .As<TService>()
                .IfNotRegistered(typeof(TService));
        }
        
        public static IRegistrationBuilder<TImplementation, SimpleActivatorData, SingleRegistrationStyle> RegisterIfNotRegistered<TService, TImplementation>(
            this ContainerBuilder builder,
            Func<IComponentContext, TImplementation> @delegate)
        
            where TImplementation : TService
        {
            return builder
                .Register(@delegate)
                .As<TService>()
                .IfNotRegistered(typeof(TService));
        }
    }
}