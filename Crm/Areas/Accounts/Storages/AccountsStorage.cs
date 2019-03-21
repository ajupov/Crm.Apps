using Crm.Areas.Accounts.Configs;
using Crm.Areas.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Storages
{
    public class AccountsStorage : DbContext
    {
        private readonly DbSqlSettings _config;

        public AccountsStorage(IOptions<DbSqlSettings> options)
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