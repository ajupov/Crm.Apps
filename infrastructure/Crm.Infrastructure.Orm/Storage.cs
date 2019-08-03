using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Infrastructure.Orm
{
    public class Storage : DbContext
    {
        private readonly OrmSettings _config;

        protected Storage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (_config.IsTestMode)
            {
                builder.UseInMemoryDatabase("main");
            }
            else
            {
                builder.UseNpgsql(_config.MainConnectionString);
            }
        }
    }
}