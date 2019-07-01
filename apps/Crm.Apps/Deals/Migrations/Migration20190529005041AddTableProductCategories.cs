using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529005041)]
    public class Migration20190529005041AddTableProductCategories : Migration
    {
        public override void Up()
        {
            Create.Table("ProductCategories")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductCategories_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_ProductCategories_AccountId_Name").OnTable("ProductCategories")
                .Columns("AccountId", "Name");

            Create.Index("IX_ProductCategories_AccountId_Name_IsDeleted_CreateDateTime").OnTable("ProductCategories")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductCategories_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("ProductCategories");
            Delete.UniqueConstraint("UQ_ProductCategories_AccountId_Key").FromTable("ProductCategories");
            Delete.Table("ProductCategories");
        }
    }
}