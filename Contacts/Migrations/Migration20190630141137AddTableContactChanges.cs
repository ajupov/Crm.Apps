using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630141137)]
    public class Migration20190630141137AddTableContactChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ContactChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ContactChanges_Id").OnTable("ContactChanges")
                .Column("Id");

            Create.Index("IX_ContactChanges_ContactId_CreateDateTime").OnTable("ContactChanges")
                .OnColumn("ContactId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactChanges_ContactId_CreateDateTime").OnTable("ContactChanges");
            Delete.PrimaryKey("PK_ContactChanges_Id").FromTable("ContactChanges");
            Delete.Table("ContactChanges");
        }
    }
}