using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface ICalculateGasPriceStrategy
    {
        Task<BigInteger> ExecuteAsync(
            IBlockchainService blockchainService);
    }
}