using System;
using Crm.Common.All.Validation.Attributes;

namespace Crm.Apps.Customers.Models
{
    public class CustomerSource
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
