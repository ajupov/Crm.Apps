using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712082800)]
    public class Migration20190712082800AddTableTaskTypes : Migration
    {
        public override void Up()
        {
            Create.Table("TaskTypes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_TaskTypes_Id").OnTable("TaskTypes")
                .Columns("Id");

            Create.UniqueConstraint("UQ_TaskTypes_AccountId_Name").OnTable("TaskTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_TaskTypes_AccountId").OnTable("TaskTypes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskTypes_AccountId").OnTable("TaskTypes");
            Delete.UniqueConstraint("UQ_TaskTypes_AccountId_Name").FromTable("TaskTypes");
            Delete.PrimaryKey("PK_TaskTypes_Id").FromTable("TaskTypes");
            Delete.Table("TaskTypes");
        }
    }
}
