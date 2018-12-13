using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface ICalculateGasAmountStrategy
    {
        Task<GasAmountCalculationResult> ExecuteAsync(
            IAddressService addressService,
            IBlockchainService blockchainService,
            string from,
            string to,
            BigInteger transferAmount);
    }
}