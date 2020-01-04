using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Accounts.Storages
{
    public class AccountsStorage : Storage
    {
        public AccountsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<AccountChange> AccountChanges { get; set; }
    }
}