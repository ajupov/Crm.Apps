using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712210630)]
    public class Migration20190712210630AddTableActivityAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_ActivityAttributes_Id").OnTable("ActivityAttributes")
                .Columns("Id");

            Create.UniqueConstraint("UQ_ActivityAttributes_AccountId_Key").OnTable("ActivityAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_ActivityAttributes_AccountId_CreateDateTime")
                .OnTable("ActivityAttributes")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityAttributes_AccountId_CreateDateTime").OnTable("ActivityAttributes");
            Delete.UniqueConstraint("UQ_ActivityAttributes_AccountId_Key").FromTable("ActivityAttributes");
            Delete.PrimaryKey("PK_ActivityAttributes_Id").FromTable("ActivityAttributes");
            Delete.Table("ActivityAttributes");
        }
    }
}