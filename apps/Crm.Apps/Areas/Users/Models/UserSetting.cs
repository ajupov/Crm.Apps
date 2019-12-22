using System;

namespace Crm.Apps.Areas.Users.Models
{
    public class UserSetting
    {
        public Guid Id { get; set; }
     
        public Guid UserId { get; set; }
        
        public UserSettingType Type { get; set; }
        
        public string Value { get; set; }
    }
}