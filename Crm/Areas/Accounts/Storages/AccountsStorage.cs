﻿using Crm.Areas.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Areas.Accounts.Storages
{
    public class AccountsStorage : DbContext
    {
        private readonly AccountsStorageSettings _config;

        public AccountsStorage(IOptions<AccountsStorageSettings> options)
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