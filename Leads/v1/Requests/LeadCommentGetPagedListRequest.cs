using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Leads.V1.Requests
{
    public class LeadCommentGetPagedListRequest
    {
        [Required]
        public Guid LeadId { get; set; }

        public DateTime? BeforeCreateDateTime { get; set; }

        public DateTime? AfterCreateDateTime { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
