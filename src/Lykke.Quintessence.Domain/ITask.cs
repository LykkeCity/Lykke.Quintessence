namespace Lykke.Quintessence.Domain
{
    public interface ITask<out T>
        where T : ITask<T>
    {
        string MessageId { get; }
        
        string PopReceipt { get; }


        T WithMessageIdAndPopReceipt(
            string messageId,
            string popReceipt);
    }
}