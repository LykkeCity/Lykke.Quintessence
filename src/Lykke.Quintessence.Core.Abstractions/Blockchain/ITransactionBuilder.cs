namespace Lykke.Quintessence.Core.Blockchain
{
    public interface ITransactionBuilder
    {
        string BuildRawTransaction(
            string serializedTxParams,
            string privateKey);
    }
}