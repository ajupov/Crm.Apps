using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Models;

namespace Crm.Apps.Settings.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        public Task<List<UserSetting>> GetListAsync(Guid userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
