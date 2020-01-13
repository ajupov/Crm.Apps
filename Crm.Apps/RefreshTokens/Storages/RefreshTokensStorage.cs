using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.RefreshTokens.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.RefreshTokens.Storages
{
    public class RefreshTokensStorage : Storage
    {
        public RefreshTokensStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}