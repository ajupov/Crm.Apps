using FluentMigrator;

namespace Crm.Apps.Stock.Migrations
{
    [Migration(20211120142802)]
    public class Migration20211120142802AddTableStockRoom : Migration
    {
        public override void Up()
        {
            Create.Table("StockRooms")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_StockRooms_Id").OnTable("StockRooms")
                .Column("Id");

            Create.UniqueConstraint("UQ_StockRooms_AccountId_Name").OnTable("StockRooms")
                .Columns("AccountId", "Name");

            Create.Index("IX_StockRooms_AccountId").OnTable("StockRooms")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_StockRooms_AccountId").OnTable("StockRooms");
            Delete.UniqueConstraint("UQ_StockRooms_AccountId_Name").FromTable("StockRooms");
            Delete.PrimaryKey("PK_StockRooms_Id").FromTable("StockRooms");
            Delete.Table("StockRooms");
        }
    }
}
