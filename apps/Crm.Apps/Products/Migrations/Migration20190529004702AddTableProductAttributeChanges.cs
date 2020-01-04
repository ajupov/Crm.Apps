using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529004702)]
    public class Migration20190529004702AddTableProductAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ProductAttributeChanges_Id").OnTable("ProductAttributeChanges")
                .Column("Id");

            Create.Index("IX_ProductAttributeChanges_AttributeId_CreateDateTime").OnTable("ProductAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductAttributeChanges_AttributeId_CreateDateTime").OnTable("ProductAttributeChanges");
            Delete.PrimaryKey("PK_ProductAttributeChanges_Id").FromTable("ProductAttributeChanges");
            Delete.Table("ProductAttributeChanges");
        }
    }
}