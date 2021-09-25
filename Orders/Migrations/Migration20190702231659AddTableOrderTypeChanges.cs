using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702231659)]
    public class Migration20190702231659AddTableOrderTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("OrderTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_OrderTypeChanges_Id").OnTable("OrderTypeChanges")
                .Column("Id");

            Create.Index("IX_OrderTypeChanges_TypeId_CreateDateTime").OnTable("OrderTypeChanges")
                .OnColumn("TypeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderTypeChanges_TypeId_CreateDateTime").OnTable("OrderTypeChanges");
            Delete.PrimaryKey("PK_OrderTypeChanges_Id").FromTable("OrderTypeChanges");
            Delete.Table("OrderTypeChanges");
        }
    }
}
