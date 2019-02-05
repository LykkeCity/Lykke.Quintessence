using JetBrains.Annotations;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    [UsedImplicitly]
    public class RootstockDetectContractStrategy : IDetectContractStrategy
    {
        public bool Execute(
            string code)
        {
            return code != "0x00";
        }
    }
}