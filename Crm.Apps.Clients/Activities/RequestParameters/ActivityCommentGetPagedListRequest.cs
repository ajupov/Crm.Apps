using System;

namespace Crm.Apps.Clients.Activities.RequestParameters
{
    public class ActivityCommentGetPagedListRequest
    {
        public Guid ActivityId { get; set; }

        public Guid? CommentatorUserId { get; set; }

        public string? Value { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; } = 0;

        public int Limit { get; set; } = 10;

        public string? SortBy { get; set; } = "CreateDateTime";

        public string? OrderBy { get; set; } = "desc";
    }
}