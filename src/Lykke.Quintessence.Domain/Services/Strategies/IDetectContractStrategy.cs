namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IDetectContractStrategy
    {
        bool Execute(
            string code);
    }
}