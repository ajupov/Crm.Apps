using FluentMigrator;

namespace Crm.Apps.Areas.Contacts.Migrations
{
    [Migration(20190630140944)]
    public class Migration20190630140944AddTableContactAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("ContactAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ContactAttributes_Id").OnTable("ContactAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_ContactAttributes_AccountId_Key").OnTable("ContactAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_ContactAttributes_AccountId").OnTable("ContactAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactAttributes_AccountId").OnTable("ContactAttributes");
            Delete.UniqueConstraint("UQ_ContactAttributes_AccountId_Key").FromTable("ContactAttributes");
            Delete.PrimaryKey("PK_ContactAttributes_Id").FromTable("ContactAttributes");
            Delete.Table("ContactAttributes");
        }
    }
}