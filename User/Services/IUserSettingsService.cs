using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.User.Models;

namespace Crm.Apps.User.Services
{
    public interface IUserSettingsService
    {
        Task<UserSetting> GetAsync(Guid userId, CancellationToken ct);
    }
}
