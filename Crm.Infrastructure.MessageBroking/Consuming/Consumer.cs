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

        public Task ConsumeAsync<T>(
            Action<Message<T>, CancellationToken> func,
            string topic,
            CancellationToken ct)
        {
            return ConsumeAsync<T>((x, y) => Task.FromResult(func), topic, ct);
        }

        public Task ConsumeAsync<T>(
            Func<Message<T>, CancellationToken, Task> action,
            string topic,
            CancellationToken ct)
        {
            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers", _host},
                {"enable.auto.commit", "false"},
                //{ "group.id", group },
            };

            using (var deserializer = new StringDeserializer(Encoding.UTF8))
            {
                using (var consumer = new Consumer<Null, string>(config, null, deserializer))
                {
                    consumer.Subscribe(topic);

                    consumer.OnMessage += async (_, message) =>
                    {
                        var deserializedMessage = JsonConvert.DeserializeObject<Message<T>>(message.Value);

                        await action(deserializedMessage, ct)
                            .ConfigureAwait(false);

                        await consumer.CommitAsync(message)
                            .ConfigureAwait(false);
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