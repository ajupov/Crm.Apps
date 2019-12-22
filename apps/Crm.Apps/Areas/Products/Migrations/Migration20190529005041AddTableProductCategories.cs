using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529005041)]
    public class Migration20190529005041AddTableProductCategories : Migration
    {
        public override void Up()
        {
            Create.Table("ProductCategories")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ProductCategories_Id").OnTable("ProductCategories")
                .Column("Id");

            Create.UniqueConstraint("UQ_ProductCategories_AccountId_Name").OnTable("ProductCategories")
                .Columns("AccountId", "Name");

            Create.Index("IX_ProductCategories_AccountId").OnTable("ProductCategories")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductCategories_AccountId").OnTable("ProductCategories");
            Delete.UniqueConstraint("UQ_ProductCategories_AccountId_Key").FromTable("ProductCategories");
            Delete.PrimaryKey("PK_ProductCategories_Id").FromTable("ProductCategories");
            Delete.Table("ProductCategories");
        }
    }
}