using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702231300)]
    public class Migration20190702231300AddTableOrderChanges : Migration
    {
        public override void Up()
        {
            Create.Table("OrderChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("OrderId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_OrderChanges_Id").OnTable("OrderChanges")
                .Column("Id");

            Create.Index("IX_OrderChanges_OrderId_CreateDateTime").OnTable("OrderChanges")
                .OnColumn("OrderId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderChanges_OrderId_CreateDateTime").OnTable("OrderChanges");
            Delete.PrimaryKey("PK_OrderChanges_Id").FromTable("OrderChanges");
            Delete.Table("OrderChanges");
        }
    }
}
