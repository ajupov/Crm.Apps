﻿using System;

namespace Crm.Apps.v1.Clients.Activities.Models
{
    public class ActivityAttributeLink
    {
        // public Guid Id { get; set; }
        //
        // public Guid ActivityId { get; set; }

        public Guid ActivityAttributeId { get; set; }

        public string Value { get; set; }
    }
}