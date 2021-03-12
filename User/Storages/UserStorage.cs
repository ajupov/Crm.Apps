using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.User.Storages
{
    public class UserStorage : Storage
    {
        public UserStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<UserSetting> UserSettings { get; set; }

        public DbSet<UserSettingChange> UserSettingChanges { get; set; }

        public DbSet<UserFlag> UserFlags { get; set; }
    }
}
