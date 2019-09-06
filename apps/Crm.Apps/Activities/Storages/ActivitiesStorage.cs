using Crm.Apps.Activities.Models;
using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
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
                e.HasKey(x => x.Id)
                    .HasName("PK_Activities_Id");

                e.HasIndex(x => new
                    {
                        x.AccountId,
                        x.TypeId,
                        x.StatusId,
                        x.LeadId,
                        x.CompanyId,
                        x.ContactId,
                        x.DealId,
                        x.CreateUserId,
                        x.ResponsibleUserId,
                        x.CreateDateTime
                    })
                    .HasName(
                        "IX_Activities_AccountId_TypeId_StatusId_LeadId_CompanyId_ContactId_DealId_CreateUserId_ResponsibleUserId_CreateDateTime");
            });

            builder.Entity<ActivityAttribute>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityAttributes_Id");

                e.HasIndex(x => new
                    {
                        x.AccountId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityAttributes_AccountId_CreateDateTime");
            });

            builder.Entity<ActivityAttributeChange>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityAttributeChanges_Id");

                e.HasIndex(x => new
                    {
                        x.ChangerUserId,
                        x.AttributeId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityAttributeChanges_ChangerUserId_AttributeId_CreateDateTime");
            });

            builder.Entity<ActivityChange>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityChanges_Id");

                e.HasIndex(x => new
                    {
                        x.ChangerUserId,
                        x.ActivityId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityChanges_ChangerUserId_ActivityId_CreateDateTime");
            });

            builder.Entity<ActivityType>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityTypes_Id");

                e.HasIndex(x => new
                    {
                        x.AccountId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityTypes_AccountId_CreateDateTime");
            });

            builder.Entity<ActivityTypeChange>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityTypeChanges_Id");

                e.HasIndex(x => new
                    {
                        x.ChangerUserId,
                        x.TypeId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityTypeChanges_ChangerUserId_TypeId_CreateDateTime");
            });

            builder.Entity<ActivityStatus>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityStatuses_Id");

                e.HasIndex(x => new
                    {
                        x.AccountId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityStatuses_AccountId_CreateDateTime");
            });

            builder.Entity<ActivityStatusChange>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityStatusChanges_Id");

                e.HasIndex(x => new
                    {
                        x.ChangerUserId,
                        x.StatusId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityStatusChanges_ChangerUserId_StatusId_CreateDateTime");
            });

            builder.Entity<ActivityComment>(e =>
            {
                e.HasKey(x => x.Id)
                    .HasName("PK_ActivityComments_Id");

                e.HasIndex(x => new
                    {
                        x.ActivityId,
                        x.CreateDateTime
                    })
                    .HasName("IX_ActivityComments_ActivityId_CreateDateTime");
            });

            builder.Entity<ActivityAttributeLink>()
                .ToTable("ActivityAttributeLinks")
                .Property(x => x.ActivityAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}