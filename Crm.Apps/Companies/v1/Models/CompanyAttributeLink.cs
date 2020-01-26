using System;
using Newtonsoft.Json;

namespace Crm.Apps.Companies.v1.Models
{
    public class CompanyAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CompanyAttributeId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}