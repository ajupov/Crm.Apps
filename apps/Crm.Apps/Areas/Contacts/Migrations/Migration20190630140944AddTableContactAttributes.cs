using FluentMigrator;

namespace Crm.Apps.Areas.Contacts.Migrations
{
    [Migration(20190630140944)]
    public class Migration20190630140944AddTableContactAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("ContactAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_ContactAttributes_AccountId_Key").OnTable("ContactAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_ContactAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("ContactAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("ContactAttributes");
            Delete.UniqueConstraint("UQ_ContactAttributes_AccountId_Key").FromTable("ContactAttributes");
            Delete.Table("ContactAttributes");
        }
    }
}