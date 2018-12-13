using System.Threading.Tasks;

namespace Lykke.Quintessence.Core.Crypto
{
    public interface IHashCalculator
    {
        byte[] Sum(
            params byte[][] data);

        Task<byte[]> SumAsync(
            params byte[][] data);
    }
}