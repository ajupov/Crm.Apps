using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public interface IConsumer
    {
        Task ConsumeAsync<T>(
            Action<Message<T>, CancellationToken> func,
            string topic,
            CancellationToken ct);

        Task ConsumeAsync<T>(
            Func<Message<T>, CancellationToken, Task> action,
            string topic,
            CancellationToken ct);
    }
}