namespace Lykke.Quintessence.Domain.Services
{
    public enum BroadcastTransactionError
    {
        BalanceIsNotEnough,
        TransactionHasBeenBroadcasted,
        TransactionHasBeenDeleted,
        TransactionShouldBeRebuilt,
        TransactionHasNotBeenFound
    }
}
