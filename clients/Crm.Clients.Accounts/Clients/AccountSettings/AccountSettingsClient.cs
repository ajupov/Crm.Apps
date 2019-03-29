using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Clients.Accounts.Clients.AccountSettings
{
    public class AccountSettingsClient : HttpClient, IAccountSettingsClient
    {
        public AccountSettingsClient(string host) : base($"{host}/api/v1/AccountSettings")
        {
        }
        
        public Task<AccountSetting> GetAsync(int accountId, AccountSettingType type)
        {
            return GetAsync<AccountSetting>("Get", new {accountId, type});
        }

        public Task<List<AccountSetting>> GetListAsync(int accountId)
        {
            return GetAsync<List<AccountSetting>>("GetList", new {accountId});
        }

        public Task SetAsync(int changerUserId, int accountId, AccountSettingType type, string value)
        {
            return PostAsync("Set", new {changerUserId, accountId, type, value});
        }

        public Task ResetAsync(int changerUserId, int accountId, AccountSettingType type)
        {
            return PostAsync("Reset", new {changerUserId, accountId, type});
        }
    }
}