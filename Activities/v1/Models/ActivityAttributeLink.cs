using System;
using Newtonsoft.Json;

namespace Crm.Apps.Activities.v1.Models
{
    public class ActivityAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ActivityId { get; set; }

        public Guid ActivityAttributeId { get; set; }

        public string Value { get; set; }
    }
}