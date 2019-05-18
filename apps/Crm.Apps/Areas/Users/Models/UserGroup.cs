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

        public List<UserGroupPermission> Permissions { get; set; }
    }
}