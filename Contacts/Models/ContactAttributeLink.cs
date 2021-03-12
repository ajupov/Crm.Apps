using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Contacts.Models
{
    public class ContactAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ContactId { get; set; }

        public Guid ContactAttributeId { get; set; }

        public string Value { get; set; }
    }
}
