using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Activities.Models;

namespace Crm.Apps.Activities.RequestParameters
{
    public class ActivityCreateRequest
    {
        [Required]
        public Guid AccountId { get; set; }
        
        [Required]
        public Guid TypeId { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        public Guid? LeadId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? ContactId { get; set; }

        public Guid? DealId { get; set; }

        public Guid? ResponsibleUserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Result { get; set; }

        [Required]
        public ActivityPriority Priority { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? DeadLineDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public List<ActivityAttributeLink>? AttributeLinks { get; set; }
    }
}