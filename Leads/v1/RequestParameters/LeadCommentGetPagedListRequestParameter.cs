using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Leads.v1.RequestParameters
{
    public class LeadCommentGetPagedListRequestParameter
    {
        [Required]
        public Guid LeadId { get; set; }

        public Guid? CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}