using System;

namespace Crm.Infrastructure.MessageBroking
{
    public class Message<T>
    {
        public string Type { get; set; }

        public Guid UserId { get; set; }

        public T Data { get; set; }
    }
}