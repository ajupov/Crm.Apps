using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Tasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Tasks.Storages
{
    public class TasksStorage : Storage
    {
        public TasksStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskAttribute> TaskAttributes { get; set; }

        public DbSet<TaskAttributeChange> TaskAttributeChanges { get; set; }

        public DbSet<TaskChange> TaskChanges { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<TaskTypeChange> TaskTypeChanges { get; set; }

        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public DbSet<TaskStatusChange> TaskStatusChanges { get; set; }

        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Task>(e =>
            {
                builder.Entity<TaskAttributeLink>()
                    .ToTable("TaskAttributeLinks")
                    .Property(x => x.TaskAttributeId)
                    .HasColumnName("AttributeId");
            });
        }
    }
}
