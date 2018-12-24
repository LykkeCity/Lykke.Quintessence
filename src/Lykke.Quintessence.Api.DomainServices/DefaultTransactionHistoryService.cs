using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Repositories;

namespace Lykke.Quintessence.Domain.Services
{
    [UsedImplicitly]
    public class DefaultTransactionHistoryService : ITransactionHistoryService
    {
        private readonly ITransactionHistoryObservationAddressesRepository _transactionHistoryObservationAddressesRepository;
        private readonly ITransactionReceiptRepository _transactionReceiptRepository;

        
        public DefaultTransactionHistoryService(
            ITransactionHistoryObservationAddressesRepository transactionHistoryObservationAddressesRepository,
            ITransactionReceiptRepository transactionReceiptRepository)
        {
            _transactionHistoryObservationAddressesRepository = transactionHistoryObservationAddressesRepository;
            _transactionReceiptRepository = transactionReceiptRepository;
        }

        /// <inheritdoc />
        public Task<bool> BeginIncomingHistoryObservationIfNotObservingAsync(
            string address)
        {
            return _transactionHistoryObservationAddressesRepository.TryAddToIncomingHistoryObservationList(address);
        }

        /// <inheritdoc />
        public Task<bool> BeginOutgoingHistoryObservationIfNotObservingAsync(
            string address)
        {
            return _transactionHistoryObservationAddressesRepository.TryAddToOutgoingHistoryObservationList(address);
        }

        /// <inheritdoc />
        public Task<bool> EndIncomingHistoryObservationIfObservingAsync(
            string address)
        {
            return _transactionHistoryObservationAddressesRepository.TryDeleteFromIncomingHistoryObservationList(address);
        }

        /// <inheritdoc />
        public Task<bool> EndOutgoingHistoryObservationIfObservingAsync(
            string address)
        {
            return _transactionHistoryObservationAddressesRepository.TryDeleteFromOutgoingHistoryObservationList(address);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TransactionReceipt>> GetIncomingHistoryAsync(
            string address,
            int take,
            string afterHash)
        {
            return GetHistoryAsync
            (
                address,
                TransactionDirection.Incoming,
                take,
                afterHash
            );
        }

        /// <inheritdoc />
        public Task<IEnumerable<TransactionReceipt>> GetOutgoingHistoryAsync(
            string address,
            int take,
            string afterHash)
        {
            return GetHistoryAsync
            (
                address,
                TransactionDirection.Outgoing,
                take,
                afterHash
            );
        }

        private async Task<IEnumerable<TransactionReceipt>> GetHistoryAsync(
            string address,
            TransactionDirection transactionDirection,
            int take,
            string afterHash)
        {
            var continuationToken = await _transactionReceiptRepository.CreateContinuationTokenAsync
            (
                address,
                transactionDirection,
                afterHash
            );

            var (transactionReceipts, _) = await _transactionReceiptRepository.GetAsync
            (
                address,
                transactionDirection,
                take,
                continuationToken
            );

            return transactionReceipts;
        }
    }
}