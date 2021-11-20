using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120144121)]
    public class Migration20211120144121AddTableStockBalanceChanges : Migration
    {
        public override void Up()
        {
            Create.Table("StockBalanceChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StockBalanceId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_StockBalanceChanges_Id").OnTable("StockBalanceChanges")
                .Column("Id");

            Create.Index("IX_StockBalanceChanges_StockBalanceId_CreateDateTime").OnTable("StockBalanceChanges")
                .OnColumn("StockBalanceId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockBalanceChanges_StockBalanceId_CreateDateTime").OnTable("StockBalanceChanges");
            Delete.PrimaryKey("PK_StockBalanceChanges_Id").FromTable("StockBalanceChanges");
            Delete.Table("StockBalanceChanges");
        }
    }
}
