using System;

namespace Crm.Apps.Settings.Models
{
    public class UserSettingChange
    {
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}
