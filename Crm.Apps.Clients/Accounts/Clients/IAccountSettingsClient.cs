using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients
{
    public interface IAccountSettingsClient
    {
        Task<Dictionary<AccountSettingType, string>> GetTypesAsync(CancellationToken ct = default);
    }
}