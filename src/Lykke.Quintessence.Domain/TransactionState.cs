namespace Lykke.Quintessence.Domain
{
    public enum TransactionState
    {
        Built,
        InProgress,
        Completed,
        Confirmed,
        Failed,
        Deleted
    }
}
