using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529004537)]
    public class Migration20190529004537AddTableProductAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("ProductAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductAttributeLinks_Id")
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_ProductAttributeLinks_ProductId")
                .FromTable("ProductAttributeLinks").ForeignColumn("ProductId")
                .ToTable("Products").PrimaryColumn("Id");

            Create.ForeignKey("FK_ProductAttributeLinks_AttributeId")
                .FromTable("ProductAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("ProductAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ProductAttributeLinks_ProductId_AttributeId").OnTable("ProductAttributeLinks")
                .Columns("ProductId", "AttributeId");

            Create.Index("IX_ProductAttributeLinks_ProductId_AttributeId_CreateDateTime").OnTable("ProductAttributeLinks")
                .OnColumn("ProductId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductAttributeLinks_ProductId_AttributeId_CreateDateTime").OnTable("ProductAttributeLinks");
            Delete.UniqueConstraint("UQ_ProductAttributeLinks_ProductId_AttributeId").FromTable("ProductAttributeLinks");
            Delete.ForeignKey("FK_ProductAttributeLinks_AttributeId").OnTable("ProductAttributeLinks");
            Delete.ForeignKey("FK_ProductAttributeLinks_ProductId").OnTable("ProductAttributeLinks");
            Delete.Table("ProductAttributeLinks");
        }
    }
}