using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529004111)]
    public class Migration20190529004111AddTableProductAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("ProductAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_ProductAttributes_AccountId_Key").OnTable("ProductAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_ProductAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("ProductAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("ProductAttributes");
            Delete.UniqueConstraint("UQ_ProductAttributes_AccountId_Key").FromTable("ProductAttributes");
            Delete.Table("ProductAttributes");
        }
    }
}