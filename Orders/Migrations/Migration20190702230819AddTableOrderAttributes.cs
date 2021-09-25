using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702230819)]
    public class Migration20190702230819AddTableOrderAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("OrderAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_OrderAttributes_Id").OnTable("OrderAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_OrderAttributes_AccountId_Key").OnTable("OrderAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_OrderAttributes_AccountId").OnTable("OrderAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderAttributes_AccountId").OnTable("OrderAttributes");
            Delete.UniqueConstraint("UQ_OrderAttributes_AccountId_Key").FromTable("OrderAttributes");
            Delete.PrimaryKey("PK_OrderAttributes_Id").FromTable("OrderAttributes");
            Delete.Table("OrderAttributes");
        }
    }
}
