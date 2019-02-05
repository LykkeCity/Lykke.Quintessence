namespace Lykke.Quintessence.Domain.Services
{
    public enum BuildTransactionError
    {
        AmountIsTooSmall,
        BalanceIsNotEnough,
        GasAmountIsInvalid,
        TargetAddressIsInvalid,
        TransactionHasBeenBroadcasted,
        TransactionHasBeenDeleted,
    }
}
