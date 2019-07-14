using Crm.Apps.Accounts.Models;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Accounts.Storages
{
    public class AccountsStorage : Storage
    {
        public AccountsStorage(
            IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<AccountChange> AccountChanges { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("PK_Accounts_Id");

                entity.HasIndex(x => x.CreateDateTime)
                    .HasName("IX_Accounts_CreateDateTime");
            });

            modelBuilder.Entity<AccountSetting>(entity =>
            {
                entity.HasKey(x => new {x.AccountId, x.Type})
                    .HasName("PK_AccountSettings_AccountId_Type");

                entity.HasIndex(x => x.AccountId)
                    .HasName("IX_AccountSettings_AccountId");
            });

            modelBuilder.Entity<AccountChange>(entity =>
            {
                entity.HasIndex(x => new {x.AccountId, x.CreateDateTime})
                    .HasName("IX_AccountChanges_AccountId_CreateDateTime");
            });
        }
    }
}