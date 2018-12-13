using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Service.BlockchainApi.Contract.Wallets;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("api/wallets")]
    public class WalletsController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletsController(
            IWalletService walletService)
        {
            _walletService = walletService;
        }
        

        [HttpPost]
        public async Task<ActionResult<WalletResponse>> CreateWallet()
        {
            var (address, privateKey) = await _walletService.CreateWalletAsync();
            
            return new WalletResponse
            {
                PrivateKey = privateKey,
                PublicAddress = address
            };
        }
    }
}
