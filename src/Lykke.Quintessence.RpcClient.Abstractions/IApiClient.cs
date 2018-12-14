using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Quintessence.RpcClient.Exceptions;
using Lykke.Quintessence.RpcClient.Models;

namespace Lykke.Quintessence.RpcClient
{
    [PublicAPI]
    public interface IApiClient
    {
        /// <summary>
        ///    Generates test transaction, executes it and returns an estimate of how much gas is necessary to allow the transaction to complete.
        ///    The transaction will not be added to the blockchain.
        /// </summary>
        /// <remarks>
        ///    Estimation may be significantly more or less than the amount of gas actually used by the transaction,
        ///    for a variety of reasons including EVM mechanics and node performance.
        /// </remarks>
        /// <param name="from">
        ///    The address the transaction is sent from.
        /// </param>
        /// <param name="to">
        ///    The address the transaction is directed to.
        /// </param>
        /// <param name="transferAmount">
        ///     Integer of the value sent with transaction.
        /// </param>
        /// <exception cref="GasAmountEstimationException">
        ///    Thrown when test transaction fails with exception.
        /// </exception>>
        /// <returns>
        ///    The amount of gas used.
        /// </returns>
        Task<BigInteger> EstimateGasAmountAsync(
            string from,
            string to,
            BigInteger transferAmount);
        
        Task<BigInteger> GetBalanceAsync(
            string address);
        
        Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber);

        Task<BigInteger> GetBestBlockNumberAsync();

        Task<Block> GetBlockAsync(
            bool includeTransactions);
        
        Task<Block> GetBlockAsync(
            string blockHash,
            bool includeTransactions);

        Task<Block> GetBlockAsync(
            BigInteger blockNumber,
            bool includeTransactions);
        
        Task<string> GetCodeAsync(
            string address);
        
        Task<BigInteger> GetGasPriceAsync();

        Task<Transaction> GetTransactionAsync(
            string transactionHash);

        Task<BigInteger> GetTransactionCountAsync(
            string address);

        Task<TransactionReceipt> GetTransactionReceiptAsync(
            string transactionHash);

        Task<string> SendRawTransactionAsync(
            string transactionData);
    }
}