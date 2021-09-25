using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702231141)]
    public class Migration20190702231141AddTableOrderAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("OrderAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_OrderAttributeChanges_Id").OnTable("OrderAttributeChanges")
                .Column("Id");

            Create.Index("IX_OrderAttributeChanges_AttributeId_CreateDateTime").OnTable("OrderAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderAttributeChanges_AttributeId_CreateDateTime").OnTable("OrderAttributeChanges");
            Delete.PrimaryKey("PK_OrderAttributeChanges_Id").FromTable("OrderAttributeChanges");
            Delete.Table("OrderAttributeChanges");
        }
    }
}
