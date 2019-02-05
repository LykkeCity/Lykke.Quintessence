using System;
using System.Threading.Tasks;
using AzureStorage.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Lykke.Quintessence.Domain.Repositories
{
    public abstract class TaskRepositoryBase<T> : ITaskRepository<T>
        where T : class, ITask<T>
    {
        private readonly IQueueExt _queue;
        
        
        protected internal TaskRepositoryBase(
            IQueueExt queue)
        {
            _queue = queue;
        }
        
        public Task CompleteAsync(
            T task)
        {
            return _queue.FinishRawMessageAsync(new CloudQueueMessage
            (
                messageId: task.MessageId,
                popReceipt: task.PopReceipt
            ));
        }

        public Task EnqueueAsync(
            T task)
        {
            return _queue.PutRawMessageAsync
            (
                JsonConvert.SerializeObject(task)
            );
        }

        public Task EnqueueAsync(
            T task,
            TimeSpan initialVisibilityDelay)
        {
            return _queue.PutRawMessageAsync
            (
                JsonConvert.SerializeObject(task),
                initialVisibilityDelay
            );
        }

        public async Task<T> TryGetAsync(
            TimeSpan visibilityTimeout)
        {
            var queueMessage = await _queue.GetRawMessageAsync((int) visibilityTimeout.TotalSeconds);

            if (queueMessage != null)
            {
                var task = JsonConvert.DeserializeObject<T>(queueMessage.AsString);

                return task.WithMessageIdAndPopReceipt
                (
                    messageId: queueMessage.Id,
                    popReceipt: queueMessage.PopReceipt
                );
            }
            else
            {
                return null;
            }
        }
    }
}