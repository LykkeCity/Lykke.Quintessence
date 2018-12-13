using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface IRootstockNonceRepository
    {
        Task InsertOrReplaceAsync(
            string address,
            BigInteger nonce);
        
        Task<BigInteger?> TryGetAsync(
            string address);
    }
}