using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120143243)]
    public class Migration20211120143243AddTableStockRoomChanges : Migration
    {
        public override void Up()
        {
            Create.Table("StockRoomChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_StockRoomChanges_Id").OnTable("StockRoomChanges")
                .Column("Id");

            Create.Index("IX_StockRoomChanges_TypeId_CreateDateTime").OnTable("StockRoomChanges")
                .OnColumn("TypeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockRoomChanges_TypeId_CreateDateTime").OnTable("StockRoomChanges");
            Delete.PrimaryKey("PK_StockRoomChanges_Id").FromTable("StockRoomChanges");
            Delete.Table("StockRoomChanges");
        }
    }
}
