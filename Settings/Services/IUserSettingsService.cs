using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public interface IUserSettingsService
    {
        Task<UserSetting> GetAsync(Guid userId, CancellationToken ct);
    }
}
