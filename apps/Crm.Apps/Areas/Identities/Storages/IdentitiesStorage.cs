using Crm.Apps.Areas.Identities.Models;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Areas.Identities.Storages
{
    public class IdentitiesStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<Identity> Identities { get; set; }

        public DbSet<IdentityToken> IdentityTokens { get; set; }

        public IdentitiesStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }
    }
}