﻿using Crm.Apps.Base.Areas.Accounts.Models;
using Crm.Infrastructure.Orm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Base.Areas.Accounts.Storages
{
    public class AccountsStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<AccountChange> AccountChanges { get; set; }

        public AccountsStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }
    }
}