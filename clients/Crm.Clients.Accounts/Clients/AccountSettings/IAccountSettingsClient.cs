using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Clients.Accounts.Clients.AccountSettings
{
    public interface IAccountSettingsClient
    {
        Task<AccountSetting> GetAsync(int accountId, AccountSettingType type);

        Task<List<AccountSetting>> GetListAsync(int accountId);

        Task SetAsync(int changerUserId, int accountId, AccountSettingType type, string value);

        Task ResetAsync(int changerUserId, int accountId, AccountSettingType type);
    }
}