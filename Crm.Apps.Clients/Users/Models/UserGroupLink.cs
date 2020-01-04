using System;

namespace Crm.Apps.Clients.Users.Models
{
    public class UserGroupLink
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid UserGroupId { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}