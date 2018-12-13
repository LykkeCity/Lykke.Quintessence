using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services
{
    public interface ISignService
    {
        [Pure, NotNull]
        string SignTransaction(
            [NotNull] string encodedTxParams,
            [NotNull] string privateKey);
    }
}