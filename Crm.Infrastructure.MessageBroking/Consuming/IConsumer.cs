using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public interface IConsumer
    {
        void Consume(
            string topic,
            Action<Message, CancellationToken> func);

        void Consume(
            string topic,
            Func<Message, CancellationToken, Task> action);

        void UnConsume();
    }
}