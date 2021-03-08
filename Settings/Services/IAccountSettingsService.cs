using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public interface IAccountSettingsService
    {
        Task<AccountSetting> GetAsync(Guid accountId, CancellationToken ct);

        Task SetActivityIndustryAsync(Guid userId, Guid accountId, AccountSettingActivityIndustry industry, CancellationToken ct);
    }
}
