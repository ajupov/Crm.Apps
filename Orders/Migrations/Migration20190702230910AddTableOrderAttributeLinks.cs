using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702230910)]
    public class Migration20190702230910AddTableOrderAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("OrderAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("OrderId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.PrimaryKey("PK_OrderAttributeLinks_Id").OnTable("OrderAttributeLinks")
                .Column("Id");

            Create.ForeignKey("FK_OrderAttributeLinks_OrderId")
                .FromTable("OrderAttributeLinks").ForeignColumn("OrderId")
                .ToTable("Orders").PrimaryColumn("Id");

            Create.ForeignKey("FK_OrderAttributeLinks_AttributeId")
                .FromTable("OrderAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("OrderAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_OrderAttributeLinks_OrderId_AttributeId").OnTable("OrderAttributeLinks")
                .Columns("OrderId", "AttributeId");

            Create.Index("IX_OrderAttributeLinks_OrderId_AttributeId").OnTable("OrderAttributeLinks")
                .OnColumn("OrderId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderAttributeLinks_OrderId_AttributeId").OnTable("OrderAttributeLinks");
            Delete.UniqueConstraint("UQ_OrderAttributeLinks_OrderId_AttributeId").FromTable("OrderAttributeLinks");
            Delete.ForeignKey("FK_OrderAttributeLinks_AttributeId").OnTable("OrderAttributeLinks");
            Delete.ForeignKey("FK_OrderAttributeLinks_OrderId").OnTable("OrderAttributeLinks");
            Delete.PrimaryKey("PK_OrderAttributeLinks_Id").FromTable("OrderAttributeLinks");
            Delete.Table("OrderAttributeLinks");
        }
    }
}
