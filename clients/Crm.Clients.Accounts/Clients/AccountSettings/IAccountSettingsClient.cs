using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients.AccountSettings
{
    public interface IAccountSettingsClient
    {
        Task<List<AccountSettingType>> GetTypesAsync(CancellationToken ct = default);
    }
}