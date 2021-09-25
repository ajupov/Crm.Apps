using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Tasks.Models
{
    public class TaskAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid TaskId { get; set; }

        public Guid TaskAttributeId { get; set; }

        public string Value { get; set; }
    }
}
