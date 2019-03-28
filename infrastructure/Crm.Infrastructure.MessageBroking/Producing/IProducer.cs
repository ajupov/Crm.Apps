using System.Threading.Tasks;
using Crm.Infrastructure.MessageBroking.Models;

namespace Crm.Infrastructure.MessageBroking.Producing
{
    public interface IProducer
    {
        Task ProduceAsync(string topic, Message message);
    }
}