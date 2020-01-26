using System;

namespace Crm.Apps.v1.Clients.Contacts.Models
{
    public class ContactAttributeLink
    {
        // public Guid Id { get; set; }
        //
        // public Guid ContactId { get; set; }

        public Guid ContactAttributeId { get; set; }

        public string Value { get; set; }
    }
}