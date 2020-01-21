using System;

namespace Crm.Apps.v1.Clients.Deals.Models
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