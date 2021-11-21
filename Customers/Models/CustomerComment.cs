using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Customers.Models
{
    public class CustomerComment
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Guid CommentatorUserId { get; set; }

        public string Value { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
