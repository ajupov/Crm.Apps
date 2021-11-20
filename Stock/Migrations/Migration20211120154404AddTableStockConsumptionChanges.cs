using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120154404)]
    public class Migration20211120154404AddTableStockConsumptionChanges : Migration
    {
        public override void Up()
        {
            Create.Table("StockConsumptionChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StockConsumptionId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_StockConsumptionChanges_Id").OnTable("StockConsumptionChanges")
                .Column("Id");

            Create.Index("IX_StockConsumptionChanges_StockConsumptionId_CreateDateTime")
                .OnTable("StockConsumptionChanges")
                .OnColumn("StockConsumptionId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockConsumptionChanges_StockConsumptionId_CreateDateTime")
                .OnTable("StockConsumptionChanges");
            Delete.PrimaryKey("PK_StockConsumptionChanges_Id").FromTable("StockConsumptionChanges");
            Delete.Table("StockConsumptionChanges");
        }
    }
}
