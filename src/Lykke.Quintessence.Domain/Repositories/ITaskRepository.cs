using System;
using System.Threading.Tasks;

namespace Lykke.Quintessence.Domain.Repositories
{
    public interface ITaskRepository<T>
        where T : ITask<T>
    {
        Task CompleteAsync(
            T task);

        Task EnqueueAsync(
            T task);

        Task EnqueueAsync(
            T task,
            TimeSpan initialVisibilityDelay);

        Task<T> TryGetAsync(
            TimeSpan visibilityTimeout);
    }
}