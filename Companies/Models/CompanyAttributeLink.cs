using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Companies.Models
{
    public class CompanyAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid CompanyId { get; set; }

        public Guid CompanyAttributeId { get; set; }

        public string Value { get; set; }
    }
}
