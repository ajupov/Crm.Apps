using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Crm.Infrastructure.MessageBroking.Consuming.Configs;

namespace Crm.Infrastructure.MessageBroking.Consuming
{
    public class Consumer : IConsumer
    {
        private readonly string _host;

        public Consumer(ConsumerConfig config)
        {
            _host = config.Host;
        }

        public Task ConsumeAsync(
            string[] topics,
            Action<string> func)
        {
            return ConsumeAsync(topics, x => Task.FromResult(func));
        }

        public Task ConsumeAsync(
            string[] topics, 
            Func<string, Task> action)
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
                    consumer.Subscribe(topics);

                    consumer.OnMessage += async (_, message) =>
                    {
                        await action(message.Value)
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