using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702224433)]
    public class Migration20190702224433AddTableOrders : Migration
    {
        public override void Up()
        {
            Create.Table("Orders")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("StartDateTime").AsDateTime2().NotNullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("Sum").AsDecimal().NotNullable()
                .WithColumn("SumWithoutDiscount").AsDecimal().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Orders_Id").OnTable("Orders")
                .Column("Id");

            Create.ForeignKey("FK_Orders_TypeId")
                .FromTable("Orders").ForeignColumn("TypeId")
                .ToTable("OrderTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Orders_StatusId")
                .FromTable("Orders").ForeignColumn("StatusId")
                .ToTable("OrderStatuses").PrimaryColumn("Id");

            Create.Index("IX_Orders_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Orders")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Orders_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Orders");
            Delete.ForeignKey("FK_Orders_TypeId").OnTable("Orders");
            Delete.ForeignKey("FK_Orders_StatusId").OnTable("Orders");
            Delete.PrimaryKey("PK_Orders_Id").FromTable("Orders");
            Delete.Table("Orders");
        }
    }
}
