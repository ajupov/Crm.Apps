using System.Threading.Tasks;

namespace Crm.Infrastructure.MessageBroking.Producing
{
    public interface IProducer
    {
        Task ProduceAsync(string topic, string message);
    }
}