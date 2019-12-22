using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Crm.Apps.Areas.Activities.Models;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid TypeId { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        [Required]
        public Guid? LeadId { get; set; }

        [Required]
        public Guid? CompanyId { get; set; }

        [Required]
        public Guid? ContactId { get; set; }

        [Required]
        public Guid? DealId { get; set; }
        
        [Required]
        public Guid? ResponsibleUserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Result { get; set; }

        [Required]
        public ActivityPriority Priority { get; set; }

        [Required]
        public DateTime? StartDateTime { get; set; }

        [Required]
        public DateTime? EndDateTime { get; set; }

        [Required]
        public DateTime? DeadLineDateTime { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
      
        [Required]
        public List<ActivityAttributeLink>? AttributeLinks { get; set; }
    }
}