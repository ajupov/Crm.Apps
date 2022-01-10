using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120153709)]
    public class Migration20211120153709AddTableStockConsumptions : Migration
    {
        public override void Up()
        {
            Create.Table("StockConsumptions")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("SupplierId").AsGuid().Nullable()
                .WithColumn("OrderId").AsGuid().Nullable()
                .WithColumn("InventoryId").AsGuid().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_StockConsumptions_Id").OnTable("StockConsumptions")
                .Column("Id");

            Create.Index("IX_StockConsumptions_AccountId_CreateDateTime").OnTable("StockConsumptions")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_StockConsumptions_AccountId_SupplierId_OrderId_InventoryId").OnTable("StockConsumptions")
                .OnColumn("AccountId").Ascending()
                .OnColumn("SupplierId").Ascending()
                .OnColumn("OrderId").Ascending()
                .OnColumn("InventoryId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockConsumptions_AccountId_SupplierId_OrderId_InventoryId").OnTable("StockConsumptions");
            Delete.Index("IX_StockConsumptions_AccountId_CreateDateTime").OnTable("StockConsumptions");
            Delete.PrimaryKey("PK_StockConsumptions_Id").FromTable("StockConsumptions");
            Delete.Table("StockConsumptions");
        }
    }
}
