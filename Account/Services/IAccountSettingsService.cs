using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Account.Models;

namespace Crm.Apps.Account.Services
{
    public interface IAccountSettingsService
    {
        Task<AccountSetting> GetAsync(Guid accountId, CancellationToken ct);

        Task SetTaskIndustryAsync(Guid userId, Guid accountId, AccountSettingTaskIndustry industry, CancellationToken ct);
    }
}
