using System.Collections.Generic;
using Crm.Apps.User.Models;

namespace Crm.Apps.User.V1.Responses
{
    public class UserSettingChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<UserSettingChange> Changes { get; set; }
    }
}
