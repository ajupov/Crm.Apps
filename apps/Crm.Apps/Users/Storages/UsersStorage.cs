using Crm.Apps.Users.Models;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Users.Storages
{
    public class UsersStorage : Storage
    {
        public UsersStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserAttribute> UserAttributes { get; set; }

        public DbSet<UserAttributeChange> UserAttributeChanges { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<UserChange> UserChanges { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<UserGroupChange> UserGroupChanges { get; set; }

        public DbSet<UserGroupLink> UserGroupLinks { get; set; }

        public DbSet<UserGroupPermission> UserGroupPermissions { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAttributeLink>()
                .ToTable("UserAttributeLinks")
                .Property(x => x.UserAttributeId)
                .HasColumnName("AttributeId");

            modelBuilder.Entity<UserGroupLink>()
                .ToTable("UserGroupLinks")
                .Property(x => x.UserGroupId)
                .HasColumnName("GroupId");

            modelBuilder.Entity<UserGroupPermission>()
                .ToTable("UserGroupPermissions")
                .Property(x => x.UserGroupId)
                .HasColumnName("GroupId");
        }
    }
}