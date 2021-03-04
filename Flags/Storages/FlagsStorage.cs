using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Flags.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Flags.Storages
{
    public class FlagsStorage : Storage
    {
        public FlagsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<AccountFlag> AccountFlags { get; set; }

        public DbSet<UserFlag> UserFlags { get; set; }
    }
}
