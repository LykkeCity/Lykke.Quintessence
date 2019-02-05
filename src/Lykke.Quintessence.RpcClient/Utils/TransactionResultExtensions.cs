using Lykke.Quintessence.RpcClient.Models;

namespace Lykke.Quintessence.RpcClient.Utils
{
    public static class TransactionResultExtensions
    {
        public static bool IsContractCreationTransaction(
            this Transaction transaction)
        {
            return transaction.To == null;
        }
        
        public static bool IsPendingTransaction(
            this Transaction transaction)
        {
            return !transaction.BlockNumber.HasValue;
        }
    }
}
