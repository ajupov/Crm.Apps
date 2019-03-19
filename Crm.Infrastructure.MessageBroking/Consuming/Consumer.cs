using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;
using Newtonsoft.Json;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public class Consumer : IConsumer
    {
        private readonly string _host;

        public Consumer(ConsumerConfig config)
        {
            _host = config.Host;
        }

        public Task ConsumeAsync(string topic,
            Action<Message, CancellationToken> func,
            CancellationToken ct)
        {
            return ConsumeAsync(topic, (x, y) => Task.FromResult(func), ct);
        }

        public Task ConsumeAsync(string topic,
            Func<Message, CancellationToken, Task> action,
            CancellationToken ct)
        {
            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers", _host},
                {"enable.auto.commit", "false"},
                { "group.id", "1" },
            };

            using (var deserializer = new StringDeserializer(Encoding.UTF8))
            {
                using (var consumer = new Consumer<Null, string>(config, null, deserializer))
                {
                    consumer.Subscribe(topic);

                    consumer.OnMessage += async (_, message) =>
                    {
                        var result = JsonConvert.DeserializeObject<Message>(message.Value);

                        await action(result, ct).ConfigureAwait(false);
                        await consumer.CommitAsync(message).ConfigureAwait(false);
                    };

                    while (true)
                    {
                        consumer.Poll(100);
                    }
                }
            }
        }
    }
}