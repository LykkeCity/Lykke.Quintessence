using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain
{
    public class BalanceMonitoringTask : ITask<BalanceMonitoringTask>
    {
        [JsonConstructor]
        public BalanceMonitoringTask(
            string address)
        {
            Address = address;
        }
        
        private BalanceMonitoringTask(
            string address,
            string messageId,
            string popReceipt)
        
            : this(address)
        {
            MessageId = messageId;
            PopReceipt = popReceipt;
        }

        
        public string Address { get; }
        
        [JsonIgnore]
        public string MessageId { get; }
        
        [JsonIgnore]
        public string PopReceipt { get; }
        
        
        public BalanceMonitoringTask WithMessageIdAndPopReceipt(
            string messageId,
            string popReceipt)
        {
            return new BalanceMonitoringTask
            (
                address: Address,
                messageId: messageId,
                popReceipt: popReceipt
            );
        }
    }
}