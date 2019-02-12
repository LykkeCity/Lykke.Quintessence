namespace Lykke.Quintessence.Domain.Services
{
    public enum BroadcastTransactionError
    {
        BalanceIsNotEnough,
        TransactionCanNotBeBroadcasted,
        TransactionHasBeenBroadcasted,
        TransactionHasBeenDeleted,
        TransactionShouldBeRebuilt,
        TransactionHasNotBeenFound
    }
}
