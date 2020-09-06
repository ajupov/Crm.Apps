using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Activities.V1.Requests
{
    public class ActivityCommentGetPagedListRequest
    {
        [Required]
        public Guid ActivityId { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;
    }
}
