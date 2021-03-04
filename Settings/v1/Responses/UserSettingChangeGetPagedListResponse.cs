using System.Collections.Generic;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.V1.Responses
{
    public class UserSettingChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<UserSettingChange> Changes { get; set; }
    }
}
