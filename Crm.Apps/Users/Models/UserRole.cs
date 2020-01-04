using System;
using Crm.Common.All.UserContext;

namespace Crm.Apps.Users.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? ModifyDateTime { get; set; }
    }
}