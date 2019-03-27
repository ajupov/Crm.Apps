using Crm.Apps.Base.Accounts.Models;
using Crm.Infrastructure.Orm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Base.Accounts.Storages
{
    public class AccountsStorage : DbContext
    {
        private readonly OrmSettings _config;

        public AccountsStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }

        public DbSet<Account> Accounts { get; set; }
    }
}