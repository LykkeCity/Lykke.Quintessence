using Lykke.AzureStorage.Tables;

namespace Lykke.Quintessence.Domain.Repositories.Entities
{
    public class BlacklistedAddressEntity : AzureTableEntity
    {
        public string Reason { get; set; }
    }
}
