using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IRootstockNonceService : INonceService
    {
        Task IncreaseNonceAsync(
            string address);
    }
}