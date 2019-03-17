using System;
using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public interface IConsumer
    {
        Task ConsumeAsync(string[] topics, Action<string> func);

        Task ConsumeAsync(string[] topics, Func<string, Task> action);
    }
}