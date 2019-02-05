namespace Lykke.Quintessence.Core.Crypto
{
    public interface IKeyGenerator
    {
        byte[] GeneratePrivateKey();

        byte[] GeneratePublicKey(
            byte[] privateKey);
    }
}