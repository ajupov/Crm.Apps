using System;

namespace Crm.Infrastructure.MessageBroking.Models
{
    public class Message
    {
        public string? Type { get; set; }

        public Guid UserId { get; set; }

        public string? Data { get; set; }
    }
}