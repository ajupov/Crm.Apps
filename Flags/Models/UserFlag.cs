using System;

namespace Crm.Apps.Flags.Models
{
    public class UserFlag
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public UserFlagType Type { get; set; }

        public DateTime SetDateTime { get; set; }
    }
}
