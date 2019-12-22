using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529010419)]
    public class Migration20190529010419AddTableProductCategoryChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductCategoryChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductCategoryChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ProductCategoryChanges_ChangerUserId_CategoryId_CreateDateTime")
                .OnTable("ProductCategoryChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("CategoryId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductCategoryChanges_ChangerUserId_CategoryId_CreateDateTime")
                .OnTable("ProductCategoryChanges");
            Delete.Table("ProductCategoryChanges");
        }
    }
}