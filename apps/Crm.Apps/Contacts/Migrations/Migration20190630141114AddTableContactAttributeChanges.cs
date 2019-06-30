using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
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

            Create.Index("IX_ContactAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ContactAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ContactAttributeChanges");
            Delete.Table("ContactAttributeChanges");
        }
    }
}