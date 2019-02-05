using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Quintessence.RpcClient.Utils
{
    public static class RpcClientExtensions
    {
        private static readonly string[] WalletCodeVariants = 
        {
            "0x0", // Ethereum, EthereumClassic
            "0x00" // Rootstock
        };
        
        
        public static async Task<bool> IsContractAsync(
            this IEthApiClient ethApiClient,
            string address)
        {
            var code = await ethApiClient.GetCodeAsync(address);

            return !WalletCodeVariants.Contains(code);
        }
        
        public static async Task<bool> IsWalletAsync(
            this IEthApiClient ethApiClient,
            string address)
        {
            var code = await ethApiClient.GetCodeAsync(address);

            return WalletCodeVariants.Contains(code);
        }
    }
}
