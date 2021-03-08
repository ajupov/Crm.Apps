using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Settings.Helpers;
using Crm.Apps.Settings.Models;
using Crm.Apps.Settings.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Settings.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly SettingsStorage _storage;

        public UserSettingsService(SettingsStorage storage)
        {
            _storage = storage;
        }

        public Task<UserSetting> GetAsync(Guid userId, CancellationToken ct)
        {
            return _storage.UserSettings
                .AsTracking()
                .SingleOrDefaultAsync(x => x.UserId == userId, ct);
        }

        private async Task SetAsync(Guid userId, Action<UserSetting> setFunction, CancellationToken ct)
        {
            var setting = await _storage.UserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

            if (setting == null)
            {
                setting = new UserSetting();
                var change = setting.CreateWithLog(userId, x =>
                {
                    x.Id = Guid.NewGuid();
                    x.UserId = userId;
                    setFunction(x);
                    x.CreateDateTime = DateTime.UtcNow;
                });

                await _storage.AddAsync(setting, ct);
                await _storage.AddAsync(change, ct);
            }
            else
            {
                var change = setting.UpdateWithLog(userId, x =>
                {
                    setFunction(x);
                    x.ModifyDateTime = DateTime.UtcNow;
                });

                _storage.Update(setting);
                await _storage.AddAsync(change, ct);
            }

            await _storage.SaveChangesAsync(ct);
        }
    }
}
