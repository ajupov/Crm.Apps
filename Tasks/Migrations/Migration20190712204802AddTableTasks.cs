using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712204802)]
    public class Migration20190712204802AddTableTasks : Migration
    {
        public override void Up()
        {
            Create.Table("Tasks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().Nullable()
                .WithColumn("CompanyId").AsGuid().Nullable()
                .WithColumn("ContactId").AsGuid().Nullable()
                .WithColumn("DealId").AsGuid().Nullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Result").AsString().Nullable()
                .WithColumn("Priority").AsByte().NotNullable()
                .WithColumn("StartDateTime").AsDateTime2().Nullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("DeadLineDateTime").AsDateTime2().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Tasks_Id").OnTable("Tasks")
                .Columns("Id");

            Create.ForeignKey("FK_Tasks_TypeId")
                .FromTable("Tasks").ForeignColumn("TypeId")
                .ToTable("TaskTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Tasks_StatusId")
                .FromTable("Tasks").ForeignColumn("StatusId")
                .ToTable("TaskStatuses").PrimaryColumn("Id");

            Create.Index("IX_Tasks_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Tasks")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Tasks_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Tasks");
            Delete.ForeignKey("FK_Tasks_TypeId").OnTable("Tasks");
            Delete.ForeignKey("FK_Tasks_StatusId").OnTable("Tasks");
            Delete.PrimaryKey("PK_Tasks_Id").FromTable("Tasks");
            Delete.Table("Tasks");
        }
    }
}
