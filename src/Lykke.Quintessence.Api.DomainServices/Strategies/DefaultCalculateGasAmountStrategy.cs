using System;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services.Utils;
using Lykke.SettingsReader;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultCalculateGasAmountStrategy : ICalculateGasAmountStrategy
    {
        private readonly SettingManager<int> _gasReserve;
        private readonly SettingManager<string, BigInteger> _maxGasAmount;
        
        
        public DefaultCalculateGasAmountStrategy(
            IReloadingManager<int> gasReserve,
            IReloadingManager<string> maxGasAmount)
        {
            _gasReserve = new SettingManager<int>
            (
                gasReserve,
                TimeSpan.FromMinutes(1)
            );
            
            _maxGasAmount = new SettingManager<string, BigInteger>
            (
                maxGasAmount,
                BigInteger.Parse,
                TimeSpan.FromMinutes(1)
            );
        }

        
        public async Task<GasAmountCalculationResult> ExecuteAsync(
            IAddressService addressService,
            IBlockchainService blockchainService,
            string from,
            string to,
            BigInteger transferAmount)
        {
            if (await blockchainService.IsContractAsync(to))
            {
                var estimatedGasAmount = await blockchainService.TryEstimateGasAmountAsync(from, to, transferAmount);

                if (estimatedGasAmount.HasValue)
                {
                    var getGasReserve = _gasReserve.GetValueAsync();
                    var getMaxGasAmount = _maxGasAmount.GetValueAsync();
                    var getCustomMaxGasAmount = addressService.TryGetCustomMaxGasAmountAsync(to);

                    await Task.WhenAll
                    (
                        getGasReserve,
                        getMaxGasAmount,
                        getCustomMaxGasAmount
                    );

                    var gasReserve = getGasReserve.Result;
                    var maxGasAmount = getMaxGasAmount.Result;
                    var customMaxGasAmount = getCustomMaxGasAmount.Result;
                    
                    if (customMaxGasAmount.HasValue && customMaxGasAmount > maxGasAmount)
                    {
                        maxGasAmount = customMaxGasAmount.Value;
                    }
                    
                    if (estimatedGasAmount.Value <= maxGasAmount)
                    {
                        var estimatedGasAmountWithReserve = estimatedGasAmount.Value * (100 + gasReserve) / 100;
                        
                        return GasAmountCalculationResult.GasAmount(estimatedGasAmountWithReserve);
                    }
                    else
                    {
                        return GasAmountCalculationResult.Error($"Gas amount [{estimatedGasAmount}] exceeds maximal [{maxGasAmount}].");
                    }
                }
                else
                {
                    return GasAmountCalculationResult.Error("Gas amount can not be estimated.");
                }
            }
            else
            {
                return GasAmountCalculationResult.GasAmount(21000);
            }
        }
    }
}