using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712210630)]
    public class Migration20190712210630AddTableTaskAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("TaskAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_TaskAttributes_Id").OnTable("TaskAttributes")
                .Columns("Id");

            Create.UniqueConstraint("UQ_TaskAttributes_AccountId_Key").OnTable("TaskAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_TaskAttributes_AccountId_CreateDateTime").OnTable("TaskAttributes")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskAttributes_AccountId_CreateDateTime").OnTable("TaskAttributes");
            Delete.UniqueConstraint("UQ_TaskAttributes_AccountId_Key").FromTable("TaskAttributes");
            Delete.PrimaryKey("PK_TaskAttributes_Id").FromTable("TaskAttributes");
            Delete.Table("TaskAttributes");
        }
    }
}
