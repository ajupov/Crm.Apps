using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public interface IAccountSettingsService
    {
        Task<AccountSetting> GetAsync(Guid accountId, AccountSettingType type, CancellationToken ct);

        Task<List<AccountSetting>> GetListAsync(Guid accountId, CancellationToken ct);
    }
}
