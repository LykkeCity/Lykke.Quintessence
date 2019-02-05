using System;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain
{
    public class TransactionMonitoringTask : ITask<TransactionMonitoringTask>
    {
        [JsonConstructor]
        public TransactionMonitoringTask(
            Guid transactionId)
        {
            TransactionId = transactionId;
        }
        
        public TransactionMonitoringTask(
            string messageId,
            string popReceipt,
            Guid transactionId)
        
            : this(transactionId)
        {
            MessageId = messageId;
            PopReceipt = popReceipt;
            TransactionId = transactionId;
        }
        
        
        [JsonIgnore]
        public string MessageId { get; }
        
        [JsonIgnore]
        public string PopReceipt { get; }
        
        public Guid TransactionId { get; }
        
        
        public TransactionMonitoringTask WithMessageIdAndPopReceipt(
            string messageId,
            string popReceipt)
        {
            return new TransactionMonitoringTask
            (
                messageId: messageId,
                popReceipt: popReceipt,
                transactionId: TransactionId
            );
        }
    }
}