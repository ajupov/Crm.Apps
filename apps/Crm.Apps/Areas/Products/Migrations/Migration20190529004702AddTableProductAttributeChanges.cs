using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529004702)]
    public class Migration20190529004702AddTableProductAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ProductAttributeChanges_ChangerUserId_AttributeId_CreateDateTime").OnTable("ProductAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductAttributeChanges_ChangerUserId_AttributeId_CreateDateTime").OnTable("ProductAttributeChanges");
            Delete.Table("ProductAttributeChanges");
        }
    }
}