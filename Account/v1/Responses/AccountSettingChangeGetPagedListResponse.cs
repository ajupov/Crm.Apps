using System.Collections.Generic;
using Crm.Apps.Account.Models;

namespace Crm.Apps.Account.V1.Responses
{
    public class AccountSettingChangeGetPagedListResponse
    {
        public int TotalCount { get; set; }

        public List<AccountSettingChange> Changes { get; set; }
    }
}
