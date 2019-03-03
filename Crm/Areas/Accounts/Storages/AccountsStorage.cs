using Crm.Areas.Accounts.Models;
using Crm.Areas.Accounts.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Storages
{
    public class AccountsStorage : DbContext
    {
        private readonly AccountsStorageSettings settings;

        public AccountsStorage(IOptions<AccountsStorageSettings> options)
        {
            settings = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(settings.ConnectionString);
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountSetting> AccountSettings { get; set; }
    }
}