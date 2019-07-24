using Crm.Apps.OAuth.Models;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.OAuth.Storages
{
    public class OAuthClientsStorage : Storage
    {
        public OAuthClientsStorage(
            IOptions<OrmSettings> options)
            : base(options)
        {
        }
        
        public DbSet<OAuthClient> Clients { get; set; }
        
        public DbSet<object> Codes { get; set; }
        
        public DbSet<object> AccessTokens { get; set; }
        
        public DbSet<object> RefreshTokens { get; set; }
    }
}