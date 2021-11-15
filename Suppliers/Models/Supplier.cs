using System;

namespace Crm.Apps.Suppliers.Models
{
    public class Supplier
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid? CreateUserId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}
