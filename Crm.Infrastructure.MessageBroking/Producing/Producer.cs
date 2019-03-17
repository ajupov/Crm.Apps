using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Crm.Infrastructure.MessageBroking.Producing.Configs;
using Microsoft.Extensions.Options;

namespace Crm.Infrastructure.MessageBroking.Producing
{
    public class Producer : IProducer
    {
        private readonly string _host;

        public Producer(IOptions<ProducerConfig> options)
        {
            _host = options.Value.Host;
        }

        public Task ProduceAsync(
            string topic,
            string message)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", _host }
            };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                return producer.ProduceAsync(topic, null, message);
            }
        }
    }
}