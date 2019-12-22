using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712082800)]
    public class Migration20190712082800AddTableActivityTypes : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityTypes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_ActivityTypes_Id").OnTable("ActivityTypes")
                .Columns("Id");

            Create.UniqueConstraint("UQ_ActivityTypes_AccountId_Name").OnTable("ActivityTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_ActivityTypes_AccountId_CreateDateTime")
                .OnTable("ActivityTypes")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityTypes_AccountId_CreateDateTime").OnTable("ActivityTypes");
            Delete.UniqueConstraint("UQ_ActivityTypes_AccountId_Name").FromTable("ActivityTypes");
            Delete.PrimaryKey("PK_ActivityTypes_Id").FromTable("ActivityTypes");
            Delete.Table("ActivityTypes");
        }
    }
}