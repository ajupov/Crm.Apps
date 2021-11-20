using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120155445)]
    public class Migration20211120155445AddTableStockArrivalChanges : Migration
    {
        public override void Up()
        {
            Create.Table("StockArrivalChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StockArrivalId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_StockArrivalChanges_Id").OnTable("StockArrivalChanges")
                .Column("Id");

            Create.Index("IX_StockArrivalChanges_StockArrivalId_CreateDateTime")
                .OnTable("StockArrivalChanges")
                .OnColumn("StockArrivalId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockArrivalChanges_StockArrivalId_CreateDateTime")
                .OnTable("StockArrivalChanges");
            Delete.PrimaryKey("PK_StockArrivalChanges_Id").FromTable("StockArrivalChanges");
            Delete.Table("StockArrivalChanges");
        }
    }
}
