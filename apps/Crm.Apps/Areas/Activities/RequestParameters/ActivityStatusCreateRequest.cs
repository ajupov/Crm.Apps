using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityStatusCreateRequest
    {
        [Required]
        public Guid AccountId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsFinish { get; set; }

        public bool IsDeleted { get; set; }
    }
}