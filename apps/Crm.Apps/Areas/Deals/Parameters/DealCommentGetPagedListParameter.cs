using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Areas.Deals.Parameters
{
    public class DealCommentGetPagedListParameter
    {
        [Required]
        public Guid DealId { get; set; }

        public Guid? CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}