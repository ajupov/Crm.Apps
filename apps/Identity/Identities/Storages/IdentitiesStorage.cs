using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Identity.Identities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Identities.Storages
{
    public class IdentitiesStorage : Storage
    {
        public IdentitiesStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Models.Identity> Identities { get; set; }

        public DbSet<IdentityToken> IdentityTokens { get; set; }
    }
}