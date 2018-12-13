using System;
using System.Linq;
using Autofac.Core;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;

namespace Lykke.Quintessence.Core.DependencyInjection
{
    [PublicAPI]
    public static class ComponentRegistryExtensions
    {
        public static bool ChainIdIsRegistered(
            this IComponentRegistry registry)
        {
            return registry
                .IsRegistered(
                    new TypedService(typeof(IChainId))
                );
        }

        public static bool ServicesAreRegistered(
            this IComponentRegistry registry,
            params Type[] types)
        {
            return types
                .Select(x => new TypedService(x))
                .All(registry.IsRegistered);
        }
    }
}