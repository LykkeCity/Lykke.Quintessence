using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface INonceService
    {
        Task<BigInteger> GetNextNonceAsync(
            string address);
    }
}