using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Activities.RequestParameters
{
    public class ActivityTypeCreateRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}