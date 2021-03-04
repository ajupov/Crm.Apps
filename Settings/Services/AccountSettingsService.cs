using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public class AccountSettingsService : IAccountSettingsService
    {
        public Task<AccountSetting> GetAsync(Guid accountId, AccountSettingType type, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountSetting>> GetListAsync(Guid accountId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
