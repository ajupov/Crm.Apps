using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUserSettingsClient
    {
        Task<Dictionary<string, UserSettingType>> GetTypesAsync(CancellationToken ct = default);
    }
}