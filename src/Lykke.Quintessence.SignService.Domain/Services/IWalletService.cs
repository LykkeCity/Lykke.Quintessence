using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IWalletService
    {
        Task<(string Address, string PrivateKey)> CreateWalletAsync();
    }
}