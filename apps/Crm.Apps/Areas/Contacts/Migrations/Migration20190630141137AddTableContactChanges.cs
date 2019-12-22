using FluentMigrator;

namespace Crm.Apps.Areas.Contacts.Migrations
{
    [Migration(20190630141137)]
    public class Migration20190630141137AddTableContactChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ContactChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ContactChanges_ChangerUserId_ContactId_CreateDateTime").OnTable("ContactChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("ContactId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactChanges_ChangerUserId_ContactId_CreateDateTime").OnTable("ContactChanges");
            Delete.Table("ContactChanges");
        }
    }
}