using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Identity.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Users.Storages
{
    public class UsersStorage : Storage
    {
        public UsersStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_Users_Id");

                e.HasIndex(x => new {x.CreateDateTime, x.IsLocked, x.IsDeleted})
                    .HasName("IX_Users_IsLocked_IsDeleted_CreateDateTime");
            });
        }
    }
}