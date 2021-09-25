using System;

namespace Crm.Apps.Orders.Models
{
    public class OrderComment
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
