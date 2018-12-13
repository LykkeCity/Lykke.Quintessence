using Lykke.Quintessence.RpcClient.Models;

namespace Lykke.Quintessence.RpcClient.Utils
{
    public static class TransactionReceiptResultExtensions
    {
        public static bool IsFailed(
            this TransactionReceipt transactionReceipt)
        {
            return transactionReceipt.Status == 0;
        }
        
        public static bool IsSucceeded(
            this TransactionReceipt transactionReceipt)
        {
            return transactionReceipt.Status == 1;
        }
    }
}
