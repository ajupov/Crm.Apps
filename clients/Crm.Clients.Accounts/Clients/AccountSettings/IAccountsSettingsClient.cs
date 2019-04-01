using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients.AccountSettings
{
    public interface IAccountsSettingsClient
    {
        Task<ICollection<AccountSettingType>> GetTypesAsync(CancellationToken ct = default);
    }
}