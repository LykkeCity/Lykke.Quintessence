using System.Threading.Tasks;

namespace Lykke.Quintessence.Core.Blockchain
{
    public interface IWalletGenerator
    {
        Task<(string Address, string AddressContext, string PrivateKey)> GenerateWalletAsync();
    }
}