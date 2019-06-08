using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529010220)]
    public class Migration20190529010220AddTableProductCategoryLinks : Migration
    {
        public override void Up()
        {
            Create.Table("ProductCategoryLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductCategoryLinks_Id")
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_ProductCategoryLinks_ProductId")
                .FromTable("ProductCategoryLinks").ForeignColumn("ProductId")
                .ToTable("Products").PrimaryColumn("Id");

            Create.ForeignKey("FK_ProductCategoryLinks_CategoryId")
                .FromTable("ProductCategoryLinks").ForeignColumn("CategoryId")
                .ToTable("ProductCategories").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ProductCategoryLinks_ProductId_CategoryId").OnTable("ProductCategoryLinks")
                .Columns("ProductId", "CategoryId");

            Create.Index("IX_ProductCategoryLinks_ProductId_CategoryId_CreateDateTime").OnTable("ProductCategoryLinks")
                .OnColumn("ProductId").Descending()
                .OnColumn("CategoryId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductCategoryLinks_ProductId_CategoryId_CreateDateTime").OnTable("ProductCategoryLinks");
            Delete.UniqueConstraint("UQ_ProductCategoryLinks_ProductId_CategoryId").FromTable("ProductCategoryLinks");
            Delete.ForeignKey("FK_ProductCategoryLinks_CategoryId").OnTable("ProductCategoryLinks");
            Delete.ForeignKey("FK_ProductCategoryLinks_ProductId").OnTable("ProductCategoryLinks");
            Delete.Table("ProductCategoryLinks");
        }
    }
}