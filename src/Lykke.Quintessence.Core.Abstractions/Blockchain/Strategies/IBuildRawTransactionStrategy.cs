namespace Lykke.Quintessence.Core.Blockchain.Strategies
{
    public interface IBuildRawTransactionStrategy
    {
        byte[] Execute(
            byte[][] transactionElements,
            byte[] privateKey);
    }
}