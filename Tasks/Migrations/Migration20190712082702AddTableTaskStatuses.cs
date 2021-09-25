using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712082702)]
    public class Migration20190712082702AddTableTaskStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("TaskStatuses")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_TaskStatuses_Id").OnTable("TaskStatuses")
                .Columns("Id");

            Create.UniqueConstraint("UQ_TaskStatuses_AccountId_Name").OnTable("TaskStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_TaskStatuses_AccountId").OnTable("TaskStatuses")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskStatuses_AccountId").OnTable("TaskStatuses");
            Delete.UniqueConstraint("UQ_TaskStatuses_AccountId_Name").FromTable("TaskStatuses");
            Delete.PrimaryKey("PK_TaskStatuses_Id").FromTable("TaskStatuses");
            Delete.Table("TaskStatuses");
        }
    }
}
