using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultCalculateTransactionAmountStrategy : ICalculateTransactionAmountStrategy
    {
        public async Task<TransactionAmountCalculationResult> ExecuteAsync(
            IBlockchainService blockchainService,
            string from,
            BigInteger transferAmount,
            BigInteger gasAmount,
            bool includeFee)
        {
            var balanceAndGasPrice = await Task.WhenAll
            (
                blockchainService.GetBalanceAsync(from),
                blockchainService.EstimateGasPriceAsync()
            );
                
            var balance = balanceAndGasPrice[0];
            var gasPrice = balanceAndGasPrice[1];
            var transactionFee = gasPrice * gasAmount;

            var transactionAmount = includeFee
                ? transferAmount - transactionFee
                : transferAmount;
            
            
            if (transactionAmount < 1)
            {
                return TransactionAmountCalculationResult.TransactionAmountIsTooSmall();
            }
                
            if (balance < transactionAmount + transactionFee)
            {
                return TransactionAmountCalculationResult.BalanceIsNotEnough();
            }
            
            return TransactionAmountCalculationResult.TransactionAmount(transactionAmount);
        }
    }
}