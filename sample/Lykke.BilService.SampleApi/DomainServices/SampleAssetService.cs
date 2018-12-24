using Lykke.Quintessence.Domain.Services;

namespace Lykke.BilService.SampleApi.DomainServices
{
    public class SampleAssetService : DefaultAssetServiceBase
    {
        public SampleAssetService() 
            
            : base(18, "", "ETH", "ETH")
        {
            
        }
    }
}