using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120161105)]
    public class Migration20211120161105AddTableStockItemUniqueElementChanges : Migration
    {
        public override void Up()
        {
            Create.Table("StockItemUniqueElementChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StockItemUniqueElementId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_StockItemUniqueElementChanges_Id").OnTable("StockItemUniqueElementChanges")
                .Column("Id");

            Create.Index("IX_StockItemUniqueElementChanges_StockItemUniqueElementId_CreateDateTime")
                .OnTable("StockItemUniqueElementChanges")
                .OnColumn("StockItemUniqueElementId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockItemUniqueElementChanges_StockItemUniqueElementId_CreateDateTime")
                .OnTable("StockItemUniqueElementChanges");
            Delete.PrimaryKey("PK_StockItemUniqueElementChanges_Id").FromTable("StockItemUniqueElementChanges");
            Delete.Table("StockItemUniqueElementChanges");
        }
    }
}
