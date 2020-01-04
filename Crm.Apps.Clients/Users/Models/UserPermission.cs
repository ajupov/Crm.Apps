using System;

namespace Crm.Apps.Clients.Users.Models
{
    public class UserPermission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Role Role { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}