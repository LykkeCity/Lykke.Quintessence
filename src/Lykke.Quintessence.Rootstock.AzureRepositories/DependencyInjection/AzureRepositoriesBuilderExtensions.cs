using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Repositories.DependencyInjection
{
    [PublicAPI]
    public static class AzureRepositoriesBuilderExtensions
    {
        public static IAzureRepositoriesRegistrant AddRootstockNonceRepository(
            this IAzureRepositoriesRegistrant registrant)
        {
            return registrant
                .RegisterDefaultRepository(RootstockNonceRepository.Create);
        }
    }
}