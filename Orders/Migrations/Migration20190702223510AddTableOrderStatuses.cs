using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702223510)]
    public class Migration20190702223510AddTableOrderStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("OrderStatuses")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_OrderStatuses_Id").OnTable("OrderStatuses")
                .Column("Id");

            Create.UniqueConstraint("UQ_OrderStatuses_AccountId_Name").OnTable("OrderStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_OrderStatuses_AccountId").OnTable("OrderStatuses")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderStatuses_AccountId").OnTable("OrderStatuses");
            Delete.UniqueConstraint("UQ_OrderStatuses_AccountId_Name").FromTable("OrderStatuses");
            Delete.PrimaryKey("PK_OrderStatuses_Id").FromTable("OrderStatuses");
            Delete.Table("OrderStatuses");
        }
    }
}
