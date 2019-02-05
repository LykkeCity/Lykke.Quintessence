using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IValidateChecksumStrategy
    {
        Task<bool> ExecuteAsync(
            string address);
    }
}