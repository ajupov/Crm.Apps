using System;
using Crm.Common.All.UserContext;

namespace Crm.Apps.Users.Models
{
    public class UserGroupRole
    {
        public Guid Id { get; set; }

        public Guid UserGroupId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}