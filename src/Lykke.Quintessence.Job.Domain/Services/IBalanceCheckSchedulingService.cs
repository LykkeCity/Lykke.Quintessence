using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Services
{
    public interface IBalanceCheckSchedulingService
    {
        Task ScheduleBalanceChecksAsync();
    }
}