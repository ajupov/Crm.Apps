using FluentMigrator;

namespace Crm.Apps.Areas.Contacts.Migrations
{
    [Migration(20190630141114)]
    public class Migration20190630141114AddTableContactAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ContactAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.PrimaryKey("PK_ContactAttributeChanges_Id").OnTable("ContactAttributeChanges")
                .Column("Id");

            Create.Index("IX_ContactAttributeChanges_AttributeId_CreateDateTime").OnTable("ContactAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactAttributeChanges_AttributeId_CreateDateTime").OnTable("ContactAttributeChanges");
            Delete.PrimaryKey("PK_ContactAttributeChanges_Id").FromTable("ContactAttributeChanges");
            Delete.Table("ContactAttributeChanges");
        }
    }
}