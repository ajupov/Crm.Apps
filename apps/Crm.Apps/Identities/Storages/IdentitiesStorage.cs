using Crm.Apps.Identities.Models;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Identities.Storages
{
    public class IdentitiesStorage : Storage
    {
        public IdentitiesStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<Identity> Identities { get; set; }

        public DbSet<IdentityToken> IdentityTokens { get; set; }

        public DbSet<IdentityChange> IdentityChanges { get; set; }
    }
}