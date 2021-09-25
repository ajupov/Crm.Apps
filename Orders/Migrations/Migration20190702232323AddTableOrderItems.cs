using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702232323)]
    public class Migration20190702232323AddTableOrderItems : Migration
    {
        public override void Up()
        {
            Create.Table("OrderItems")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("OrderId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("ProductName").AsString(64).NotNullable()
                .WithColumn("ProductVendorCode").AsString(16).Nullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .WithColumn("Count").AsDecimal().NotNullable();

            Create.PrimaryKey("PK_OrderItems_Id").OnTable("OrderItems")
                .Column("Id");

            Create.ForeignKey("FK_OrderItems_OrderId")
                .FromTable("OrderItems").ForeignColumn("OrderId")
                .ToTable("Orders").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_OrderItems_OrderId_ProductId").OnTable("OrderItems")
                .Columns("OrderId", "ProductId");

            Create.Index("IX_OrderItems_OrderId").OnTable("OrderItems")
                .OnColumn("OrderId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderItems_OrderId").OnTable("OrderItems");
            Delete.UniqueConstraint("UQ_OrderItems_OrderId_ProductId").FromTable("OrderItems");
            Delete.ForeignKey("FK_OrderItems_OrderId").OnTable("OrderItems");
            Delete.PrimaryKey("PK_OrderItems_Id").FromTable("OrderItems");
            Delete.Table("OrderItems");
        }
    }
}
