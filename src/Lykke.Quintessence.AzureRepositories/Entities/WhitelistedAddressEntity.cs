using System.Numerics;
using Lykke.AzureStorage.Tables;

namespace Lykke.Quintessence.Domain.Repositories.Entities
{
    public class WhitelistedAddressEntity : AzureTableEntity
    {
        public BigInteger MaxGasAmount { get; set; }
        
    }
}
