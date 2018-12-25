using System.Linq;
using JetBrains.Annotations;
using Nethereum.Signer.Crypto;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Lykke.Quintessence.Core.Crypto
{
    [UsedImplicitly, PublicAPI]
    public class DefaultKeyGenerator : IKeyGenerator
    {
        private readonly SecureRandom _secureRandom;

        public DefaultKeyGenerator()
        {
            _secureRandom = new SecureRandom();
        }
        
        public byte[] GeneratePrivateKey()
        {
            while (true)
            {
                var generator = new ECKeyPairGenerator("EC");
                var generatorInitParams = new KeyGenerationParameters(_secureRandom, 256);

                generator.Init(generatorInitParams);

                var keyPair = generator.GenerateKeyPair();
                var privateBytes = ((ECPrivateKeyParameters) keyPair.Private).D.ToByteArray();

                if (privateBytes.Length == 32)
                {
                    return privateBytes;
                }
            }
        }

        public byte[] GeneratePublicKey(
            byte[] privateKey)
        {
            var ecKey = new ECKey(privateKey, true);

            return ecKey
                .GetPubKey(false)
                .Skip(1) // Skipping prefix
                .ToArray();
        }
    }
}
