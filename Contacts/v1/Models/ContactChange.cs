using System;

namespace Crm.Apps.Contacts.v1.Models
{
    public class ContactChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid ContactId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}