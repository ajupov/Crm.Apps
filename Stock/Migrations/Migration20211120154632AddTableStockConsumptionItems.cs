using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120154632)]
    public class Migration20211120154632AddTableStockConsumptionItems : Migration
    {
        public override void Up()
        {
            Create.Table("StockConsumptionItems")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("StockConsumptionId").AsGuid().NotNullable()
                .WithColumn("RoomId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("Count").AsDecimal().NotNullable();

            Create.PrimaryKey("PK_StockConsumptionItems_Id").OnTable("StockConsumptionItems")
                .Column("Id");

            Create.ForeignKey("FK_StockConsumptionItems_StockConsumptionId")
                .FromTable("StockConsumptionItems").ForeignColumn("StockConsumptionId")
                .ToTable("StockConsumptions").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_StockConsumptionItems_StockConsumptionId_RoomId_ProductId")
                .OnTable("StockConsumptionItems")
                .Columns("StockConsumptionId", "RoomId", "ProductId");

            Create.Index("IX_StockConsumptionItems_StockConsumptionId").OnTable("StockConsumptionItems")
                .OnColumn("StockConsumptionId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockConsumptionItems_StockConsumptionId").OnTable("StockConsumptionItems");
            Delete.UniqueConstraint("UQ_StockConsumptionItems_StockConsumptionId_RoomId_ProductId")
                .FromTable("StockConsumptionItems");
            Delete.ForeignKey("FK_StockConsumptionItems_StockConsumptionId").OnTable("StockConsumptionItems");
            Delete.PrimaryKey("PK_StockConsumptionItems_Id").FromTable("StockConsumptionItems");
            Delete.Table("StockConsumptionItems");
        }
    }
}
