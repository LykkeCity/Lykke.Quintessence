using System;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultBuildTransactionStrategy : IBuildTransactionStrategy
    {
        private readonly ICalculateGasPriceStrategy _calculateGasPriceStrategy;
        
        
        public DefaultBuildTransactionStrategy(
            ICalculateGasPriceStrategy calculateGasPriceStrategy)
        {
            _calculateGasPriceStrategy = calculateGasPriceStrategy;
        }

        
        public async Task<string> ExecuteAsync(
            ITransactionRepository transactionRepository,
            IBlockchainService blockchainService,
            Guid transactionId,
            string from,
            string to,
            BigInteger transactionAmount,
            BigInteger gasAmount,
            bool includeFee)
        {
            var gasPrice = await _calculateGasPriceStrategy.ExecuteAsync
            (
                blockchainService
            );
            
            var transactionData = await blockchainService.BuildTransactionAsync
            (
                from: from,
                to: to,
                amount: transactionAmount,
                gasAmount: gasAmount,
                gasPrice: gasPrice
            );
                
            await transactionRepository.AddAsync(Transaction.Create
            (
                transactionId: transactionId,
                from: from,
                to: to,
                amount: transactionAmount,
                gasAmount: gasAmount,
                gasPrice: gasPrice,
                includeFee: includeFee,
                data: transactionData
            ));
            
            return transactionData;
        }
    }
}