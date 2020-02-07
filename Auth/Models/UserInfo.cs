using System.Collections.Generic;

namespace Crm.Apps.Auth.Models
{
    public class UserInfo
    {
        public string Name { get; set; }

        public List<string> Roles { get; set; }
    }
}