using FluentMigrator;

namespace Crm.Apps.Products.Migrations
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
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ProductCategoryChanges_Id").OnTable("ProductCategoryChanges")
                .Column("Id");

            Create.Index("IX_ProductCategoryChanges_CategoryId_CreateDateTime").OnTable("ProductCategoryChanges")
                .OnColumn("CategoryId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductCategoryChanges_CategoryId_CreateDateTime").OnTable("ProductCategoryChanges");
            Delete.PrimaryKey("PK_ProductCategoryChanges_Id").FromTable("ProductCategoryChanges");
            Delete.Table("ProductCategoryChanges");
        }
    }
}