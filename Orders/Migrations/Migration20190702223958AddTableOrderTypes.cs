using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702223958)]
    public class Migration20190702223958AddTableOrderTypes : Migration
    {
        public override void Up()
        {
            Create.Table("OrderTypes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_OrderTypes_Id").OnTable("OrderTypes")
                .Column("Id");

            Create.UniqueConstraint("UQ_OrderTypes_AccountId_Name").OnTable("OrderTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_OrderTypes_AccountId").OnTable("OrderTypes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderTypes_AccountId").OnTable("OrderTypes");
            Delete.UniqueConstraint("UQ_OrderTypes_AccountId_Name").FromTable("OrderTypes");
            Delete.PrimaryKey("PK_OrderTypes_Id").FromTable("OrderTypes");
            Delete.Table("OrderTypes");
        }
    }
}
