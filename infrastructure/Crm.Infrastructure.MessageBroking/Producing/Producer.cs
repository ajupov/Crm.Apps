using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using Crm.Infrastructure.MessageBroking.Models;
using Crm.Infrastructure.MessageBroking.Settings;
using Crm.Utils.Json;
using Crm.Utils.String;
using Microsoft.Extensions.Options;

namespace Crm.Infrastructure.MessageBroking.Producing
{
    public class Producer : IProducer
    {
        private readonly KeyValuePair<string, string>[] _config;

        public Producer(IOptions<MessageBrokingProducerSettings> options)
        {
            if (options.Value.Host == null || options.Value.Host.IsEmpty())
            {
                throw new ArgumentException("Host is null or empty", options.Value.Host);
            }

            _config = new[]
            {
                new KeyValuePair<string, string>("bootstrap.servers", options.Value.Host)
            };
        }

        public Task ProduceAsync(string topic, Message message)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();

            return producer.ProduceAsync(topic, new Message<Null, string> {Value = message.ToJsonString()});
        }
    }
}