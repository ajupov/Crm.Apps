using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712211941)]
    public class Migration20190712211941AddTableTaskStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("TaskStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_TaskStatusChanges_Id").OnTable("TaskStatusChanges")
                .Columns("Id");

            Create.Index("IX_TaskStatusChanges_StatusId_CreateDateTime").OnTable("TaskStatusChanges")
                .OnColumn("StatusId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskStatusChanges_StatusId_CreateDateTime").OnTable("TaskStatusChanges");
            Delete.PrimaryKey("PK_TaskStatusChanges_Id").FromTable("TaskStatusChanges");
            Delete.Table("TaskStatusChanges");
        }
    }
}
