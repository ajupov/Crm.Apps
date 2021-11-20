using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120155502)]
    public class Migration20211120155502AddTableStockArrivalItems : Migration
    {
        public override void Up()
        {
            Create.Table("StockArrivalItems")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("StockArrivalId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("Count").AsDecimal().NotNullable();

            Create.PrimaryKey("PK_StockArrivalItems_Id").OnTable("StockArrivalItems")
                .Column("Id");

            Create.ForeignKey("FK_StockArrivalItems_StockArrivalId")
                .FromTable("StockArrivalItems").ForeignColumn("StockArrivalId")
                .ToTable("StockArrivals").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_StockArrivalItems_StockArrivalId_ProductId")
                .OnTable("StockArrivalItems")
                .Columns("StockArrivalId", "ProductId");

            Create.Index("IX_StockArrivalItems_StockArrivalId").OnTable("StockArrivalItems")
                .OnColumn("StockArrivalId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockArrivalItems_StockArrivalId").OnTable("StockArrivalItems");
            Delete.UniqueConstraint("UQ_StockArrivalItems_StockArrivalId_ProductId")
                .FromTable("StockArrivalItems");
            Delete.ForeignKey("FK_StockArrivalItems_StockArrivalId").OnTable("StockArrivalItems");
            Delete.PrimaryKey("PK_StockArrivalItems_Id").FromTable("StockArrivalItems");
            Delete.Table("StockArrivalItems");
        }
    }
}
