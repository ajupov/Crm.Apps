using System;

namespace Identity.Clients.Models
{
    public class ClientScope
    {
        public ClientScope(Guid clientId, string value)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            Value = value;
        }

        public Guid Id { get; }

        public Guid ClientId { get; }

        public string Value { get; }
    }
}