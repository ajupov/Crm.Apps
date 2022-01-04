using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120143532)]
    public class Migration20211120143532AddTableStockBalances : Migration
    {
        public override void Up()
        {
            Create.Table("StockBalances")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("RoomId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("Count").AsDecimal().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_StockBalances_Id").OnTable("StockBalances")
                .Column("Id");

            Create.Index("IX_StockBalances_AccountId_CreateUserId_CreateDateTime").OnTable("StockBalances")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_StockBalances_AccountId_RoomId_ProductId").OnTable("StockBalances")
                .OnColumn("AccountId").Ascending()
                .OnColumn("RoomId").Ascending()
                .OnColumn("ProductId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockBalances_AccountId_RoomId_ProductId").OnTable("StockBalances");
            Delete.Index("IX_StockBalances_AccountId_CreateUserId_CreateDateTime").OnTable("StockBalances");
            Delete.PrimaryKey("PK_StockBalances_Id").FromTable("StockBalances");
            Delete.Table("StockBalances");
        }
    }
}
