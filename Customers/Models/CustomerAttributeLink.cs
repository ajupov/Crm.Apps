﻿using System;
using System.Text.Json.Serialization;

namespace Crm.Apps.Customers.Models
{
    public class CustomerAttributeLink
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid CustomerId { get; set; }

        public Guid CustomerAttributeId { get; set; }

        public string Value { get; set; }
    }
}
