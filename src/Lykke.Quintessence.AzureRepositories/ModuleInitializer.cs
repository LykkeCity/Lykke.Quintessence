using System.Numerics;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables.Entity.Metamodel;
using Lykke.AzureStorage.Tables.Entity.Metamodel.Providers;
using Lykke.Quintessence.Domain.Repositories.Serializers;

namespace Lykke.Quintessence.Domain.Repositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal static class ModuleInitializer
    {
        /// <summary>
        ///    This method is called from assembly's module constructor.
        /// </summary>
        public static void Initialize()
        {
            EntityMetamodel.Configure
            (
                provider: CreateMetamodelProvider()
            );
        }
        
        private static IMetamodelProvider CreateMetamodelProvider()
        {
            return new CompositeMetamodelProvider()
                .AddProvider
                (
                    new AnnotationsBasedMetamodelProvider()
                )
                .AddProvider
                (
                    new ConventionBasedMetamodelProvider()
                        .AddTypeSerializerRule
                        (
                            t => t == typeof(BigInteger),
                            s => new BigIntegerSerializer()
                        )
                );
        }
    }
}