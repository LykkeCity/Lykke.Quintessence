using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services.Strategies
{
    public interface IAddChecksumStrategy
    {
        Task<string> ExecuteAsync(
            string address);
    }
}