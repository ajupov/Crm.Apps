using System;

namespace Crm.Apps.Clients.Users.Models
{
    public class UserAttributeLink
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid UserAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}