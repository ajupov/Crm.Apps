using System;

namespace Crm.Clients.Users.Models
{
    public class UserPostLink
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid PostId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}