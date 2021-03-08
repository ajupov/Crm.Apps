using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Settings.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Settings.Storages
{
    public class SettingsStorage : Storage
    {
        public SettingsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<AccountSettingChange> AccountSettingChanges { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<UserSettingChange> UserSettingChanges { get; set; }
    }
}
