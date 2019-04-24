using System;

namespace Crm.Clients.Users.Models
{
    public class UserPostChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid PostId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}