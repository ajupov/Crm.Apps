using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Activities.v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Activities.Storages
{
    public class ActivitiesStorage : Storage
    {
        public ActivitiesStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityAttribute> ActivityAttributes { get; set; }

        public DbSet<ActivityAttributeChange> ActivityAttributeChanges { get; set; }

        public DbSet<ActivityChange> ActivityChanges { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<ActivityTypeChange> ActivityTypeChanges { get; set; }

        public DbSet<ActivityStatus> ActivityStatuses { get; set; }

        public DbSet<ActivityStatusChange> ActivityStatusChanges { get; set; }

        public DbSet<ActivityComment> ActivityComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Activity>(e =>
            {
                builder.Entity<ActivityAttributeLink>()
                    .ToTable("ActivityAttributeLinks")
                    .Property(x => x.ActivityAttributeId)
                    .HasColumnName("AttributeId");
            });
        }
    }
}