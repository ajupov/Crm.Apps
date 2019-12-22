using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityStatusUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsFinish { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}