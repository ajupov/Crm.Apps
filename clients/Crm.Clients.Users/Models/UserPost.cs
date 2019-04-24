using System;
using System.Collections.Generic;

namespace Crm.Clients.Users.Models
{
    public class UserPost
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ICollection<UserPostLink> Links { get; set; }

        public ICollection<UserPostChange> Changes { get; set; }
    }
}