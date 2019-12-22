using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Activities.RequestParameters
{
    public class ActivityCommentCreateRequest
    {
        [Required]
        public Guid ActivityId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}