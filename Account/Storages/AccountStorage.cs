using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Account.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Account.Storages
{
    public class AccountStorage : Storage
    {
        public AccountStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<AccountSettingChange> AccountSettingChanges { get; set; }

        public DbSet<AccountFlag> AccountFlags { get; set; }
    }
}
