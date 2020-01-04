using System;

namespace Crm.Apps.Clients.Users.Models
{
    public class UserGroupChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid GroupId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}