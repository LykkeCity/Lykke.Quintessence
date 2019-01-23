using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IBlockchainService
    {
        [ItemNotNull]
        Task<string> BroadcastTransactionAsync(
            [NotNull] string signedTxData);
        
        [ItemNotNull]
        string BuildTransaction(
            [NotNull] string from,
            [NotNull] string to,
            BigInteger amount,
            BigInteger gasAmount,
            BigInteger gasPrice,
            BigInteger nonce);
        
        Task<BigInteger> EstimateGasPriceAsync();
        
        Task<BigInteger> GetBalanceAsync(
            [NotNull] string address);
        
        Task<BigInteger> GetBalanceAsync(
            [NotNull] string address,
            BigInteger blockNumber);

        Task<int> GetConfirmationLevel();
        
        Task<BigInteger> GetBestTrustedBlockNumberAsync();

        [ItemNotNull]
        Task<IEnumerable<TransactionReceipt>> GetTransactionReceiptsAsync(
            BigInteger blockNumber);

        Task<bool> IsContractAsync(
            [NotNull] string address);

        [ItemCanBeNull]
        Task<BigInteger?> TryEstimateGasAmountAsync(
            [NotNull] string from,
            [NotNull] string to,
            BigInteger amount);

        [ItemCanBeNull]
        Task<TransactionResult> GetTransactionResultAsync(
            [NotNull] string hash);
    }
}
