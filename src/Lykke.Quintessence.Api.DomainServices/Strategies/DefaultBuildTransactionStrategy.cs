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
        private readonly IBlockchainService _blockchainService;
        private readonly INonceService _nonceService;
        private readonly ITransactionRepository _transactionRepository;


        public DefaultBuildTransactionStrategy(
            IBlockchainService blockchainService,
            INonceService nonceService,
            ITransactionRepository transactionRepository)
        {
            _blockchainService = blockchainService;
            _nonceService = nonceService;
            _transactionRepository = transactionRepository;
        }

        
        public async Task<string> ExecuteAsync(
            Guid transactionId,
            string from,
            string to,
            BigInteger transactionAmount,
            BigInteger gasAmount,
            bool includeFee)
        {
            var gasPrice = await _blockchainService.EstimateGasPriceAsync();
            var nonce = await _nonceService.GetNextNonceAsync(from);
            
            var transactionData = _blockchainService.BuildTransaction
            (
                from: from,
                to: to,
                amount: transactionAmount,
                gasAmount: gasAmount,
                gasPrice: gasPrice,
                nonce: nonce
            );
                
            await _transactionRepository.AddAsync(Transaction.Create
            (
                transactionId: transactionId,
                from: from,
                to: to,
                amount: transactionAmount,
                gasAmount: gasAmount,
                gasPrice: gasPrice,
                includeFee: includeFee,
                nonce: nonce,
                data: transactionData
            ));
            
            return transactionData;
        }
    }
}