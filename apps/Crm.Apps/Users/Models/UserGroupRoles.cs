using System;
using Crm.Common.UserContext;

namespace Crm.Apps.Users.Models
{
    public class UserGroupRoles
    {
        public Guid Id { get; set; }

        public Guid UserGroupId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}