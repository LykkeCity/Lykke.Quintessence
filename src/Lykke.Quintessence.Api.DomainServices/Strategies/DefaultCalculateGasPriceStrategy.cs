using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultCalculateGasPriceStrategy : ICalculateGasPriceStrategy
    {
        public Task<BigInteger> ExecuteAsync(
            IBlockchainService blockchainService)
        {
            return blockchainService.EstimateGasPriceAsync();
        }
    }
}