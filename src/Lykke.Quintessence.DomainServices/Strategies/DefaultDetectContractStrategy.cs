using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class DefaultDetectContractStrategy : IDetectContractStrategy
    {
        public bool Execute(
            string code)
        {
            return code != "0x";
        }
    }
}