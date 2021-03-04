using System.Collections.Generic;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.V1.Responses
{
    public class AccountSettingChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<AccountSettingChange> Changes { get; set; }
    }
}
