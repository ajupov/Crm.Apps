﻿using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Tasks.Models
{
    public class TaskAttributeChange
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid AttributeId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}
