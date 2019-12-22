using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529004111)]
    public class Migration20190529004111AddTableProductAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("ProductAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ProductAttributes_Id").OnTable("ProductAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_ProductAttributes_AccountId_Key").OnTable("ProductAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_ProductAttributes_AccountId").OnTable("ProductAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductAttributes_AccountId").OnTable("ProductAttributes");
            Delete.UniqueConstraint("UQ_ProductAttributes_AccountId_Key").FromTable("ProductAttributes");
            Delete.PrimaryKey("PK_ProductAttributes_Id").FromTable("ProductAttributes");
            Delete.Table("ProductAttributes");
        }
    }
}