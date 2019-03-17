using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Producing
{
    public interface IProducer
    {
        Task ProduceAsync<T>(
            string topic,
            Message<T> message);
    }
}