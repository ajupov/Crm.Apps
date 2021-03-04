using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public interface IUserSettingsService
    {
        Task<List<UserSetting>> GetListAsync(Guid userId, CancellationToken ct);
    }
}
