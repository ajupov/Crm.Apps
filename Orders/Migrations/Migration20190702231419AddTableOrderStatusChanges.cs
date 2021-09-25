using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702231419)]
    public class Migration20190702231419AddTableOrderStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("OrderStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_OrderStatusChanges_Id").OnTable("OrderStatusChanges")
                .Column("Id");

            Create.Index("IX_OrderStatusChanges_StatusId_CreateDateTime").OnTable("OrderStatusChanges")
                .OnColumn("StatusId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderStatusChanges_StatusId_CreateDateTime").OnTable("OrderStatusChanges");
            Delete.PrimaryKey("PK_OrderStatusChanges_Id").FromTable("OrderStatusChanges");
            Delete.Table("OrderStatusChanges");
        }
    }
}
