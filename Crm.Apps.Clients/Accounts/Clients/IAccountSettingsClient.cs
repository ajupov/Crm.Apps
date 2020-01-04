using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Accounts.Models;

namespace Crm.Apps.Clients.Accounts.Clients
{
    public interface IAccountSettingsClient
    {
        Task<Dictionary<AccountSettingType, string>> GetTypesAsync(CancellationToken ct = default);
    }
}