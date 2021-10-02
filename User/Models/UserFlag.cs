using System;

namespace Crm.Apps.User.Models
{
    public class UserFlag
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public UserFlagType Type { get; set; }

        public DateTime SetDateTime { get; set; }
    }
}
