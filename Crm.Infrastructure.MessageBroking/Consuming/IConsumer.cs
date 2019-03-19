using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public interface IConsumer
    {
        Task ConsumeAsync(string topic,
            Action<Message, CancellationToken> func,
            CancellationToken ct);

        Task ConsumeAsync(string topic,
            Func<Message, CancellationToken, Task> action,
            CancellationToken ct);
    }
}