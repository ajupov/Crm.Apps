using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120155434)]
    public class Migration20211120155434AddTableStockArrivals : Migration
    {
        public override void Up()
        {
            Create.Table("StockArrivals")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("OrderId").AsGuid().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_StockArrivals_Id").OnTable("StockArrivals")
                .Column("Id");

            Create.Index("IX_StockArrivals_AccountId_CreateDateTime").OnTable("StockArrivals")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_StockArrivals_AccountId_OrderId").OnTable("StockArrivals")
                .OnColumn("AccountId").Ascending()
                .OnColumn("OrderId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockArrivals_AccountId_OrderId").OnTable("StockArrivals");
            Delete.Index("IX_StockArrivals_AccountId_CreateDateTime").OnTable("StockArrivals");
            Delete.PrimaryKey("PK_StockArrivals_Id").FromTable("StockArrivals");
            Delete.Table("StockArrivals");
        }
    }
}
