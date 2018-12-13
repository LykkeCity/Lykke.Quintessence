using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.Utils;

namespace Lykke.Quintessence.Core.Blockchain
{
    [UsedImplicitly]
    public class DefaultWalletGenerator : IWalletGenerator
    {
        private readonly IHashCalculator _hashCalculator;
        private readonly IKeyGenerator _keyGenerator;

        public DefaultWalletGenerator(
            IHashCalculator hashCalculator,
            IKeyGenerator keyGenerator)
        {
            _hashCalculator = hashCalculator;
            _keyGenerator = keyGenerator;
        }


        public async Task<(string Address, string PrivateKey)> GenerateWalletAsync()
        {
            var privateKey = _keyGenerator.GeneratePrivateKey();
            var publicKey = _keyGenerator.GeneratePublicKey(privateKey);
            var address = (await _hashCalculator.SumAsync(publicKey.Slice(1))).Slice(12, 32);

            return (address.ToHexString(), privateKey.ToHexString());
        }
    }
}