using System;
using Crm.Common.UserContext;

namespace Crm.Clients.Users.Models
{
    public class UserPermission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}