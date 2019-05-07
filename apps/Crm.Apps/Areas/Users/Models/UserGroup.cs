using System;
using System.Collections.Generic;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserGroup
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime CreateDateTime { get; set; }

        public ICollection<UserGroupLink> Links { get; set; }

        public ICollection<UserGroupPermission> Permissions { get; set; }
        
        public ICollection<UserGroupChange> Changes { get; set; }
    }
}