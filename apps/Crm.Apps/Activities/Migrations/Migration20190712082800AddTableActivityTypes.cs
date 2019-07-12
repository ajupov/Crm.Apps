using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712082800)]
    public class Migration20190712082800AddTableActivityTypes : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityTypes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityTypes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_ActivityTypes_AccountId_Name").OnTable("ActivityTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_ActivityTypes_AccountId_Name_IsDeleted_CreateDateTime")
                .OnTable("ActivityTypes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityTypes_AccountId_Name_IsDeleted_CreateDateTime").OnTable("ActivityTypes");
            Delete.UniqueConstraint("UQ_ActivityTypes_AccountId_Name").FromTable("ActivityTypes");
            Delete.Table("ActivityTypes");
        }
    }
}