using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120160311)]
    public class Migration20211120160311AddTableStockItemUniqueElement : Migration
    {
        public override void Up()
        {
            Create.Table("StockItemUniqueElements")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("Status").AsByte().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_StockItemUniqueElements_Id").OnTable("StockItemUniqueElements")
                .Column("Id");

            Create.UniqueConstraint("UQ_StockItemUniqueElements_AccountId_ProductId_Value")
                .OnTable("StockItemUniqueElements")
                .Columns("AccountId", "ProductId", "Value");

            Create.Index("IX_StockItemUniqueElements_AccountId_ProductId").OnTable("StockItemUniqueElements")
                .OnColumn("AccountId").Ascending()
                .OnColumn("ProductId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockItemUniqueElements_AccountId_ProductId").OnTable("StockItemUniqueElements");
            Delete.UniqueConstraint("UQ_StockItemUniqueElements_AccountId_ProductId_Value")
                .FromTable("StockItemUniqueElements");
            Delete.PrimaryKey("PK_StockItemUniqueElements_Id").FromTable("StockItemUniqueElements");
            Delete.Table("StockItemUniqueElements");
        }
    }
}
