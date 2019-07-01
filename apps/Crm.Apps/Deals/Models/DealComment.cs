using System;

namespace Crm.Apps.Deals.Models
{
    public class DealComment
    {
        public Guid Id { get; set; }

        public Guid DealId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}