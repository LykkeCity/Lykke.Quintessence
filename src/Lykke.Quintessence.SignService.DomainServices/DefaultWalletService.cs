using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultWalletService : IWalletService
    {
        private readonly IAddChecksumStrategy _addChecksumStrategy;
        private readonly IWalletGenerator _walletGenerator;

        
        public DefaultWalletService(
            IAddChecksumStrategy addChecksumStrategy,
            IWalletGenerator walletGenerator)
        {
            _addChecksumStrategy = addChecksumStrategy;
            _walletGenerator = walletGenerator;
        }

        
        public async Task<(string Address, string AddressContext, string PrivateKey)> CreateWalletAsync()
        {
            var (address, addressContext, privateKey) = await _walletGenerator.GenerateWalletAsync();

            return (await _addChecksumStrategy.ExecuteAsync(address), addressContext, privateKey);
        }
    }
}