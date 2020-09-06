using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Deals.V1.Requests
{
    public class DealCommentGetPagedListRequest
    {
        [Required]
        public Guid DealId { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;
    }
}
