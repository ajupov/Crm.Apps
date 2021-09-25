using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620115401)]
    public class Migration20190620115401AddTableCustomerAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.PrimaryKey("PK_CustomerAttributeLinks_Id").OnTable("CustomerAttributeLinks")
                .Column("Id");

            Create.ForeignKey("FK_CustomerAttributeLinks_CustomerId")
                .FromTable("CustomerAttributeLinks").ForeignColumn("CustomerId")
                .ToTable("Customers").PrimaryColumn("Id");

            Create.ForeignKey("FK_CustomerAttributeLinks_AttributeId")
                .FromTable("CustomerAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("CustomerAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_CustomerAttributeLinks_CustomerId_AttributeId").OnTable("CustomerAttributeLinks")
                .Columns("CustomerId", "AttributeId");

            Create.Index("IX_CustomerAttributeLinks_CustomerId_AttributeId").OnTable("CustomerAttributeLinks")
                .OnColumn("CustomerId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerAttributeLinks_CustomerId_AttributeId").OnTable("CustomerAttributeLinks");
            Delete.UniqueConstraint("UQ_CustomerAttributeLinks_CustomerId_AttributeId").FromTable("CustomerAttributeLinks");
            Delete.ForeignKey("FK_CustomerAttributeLinks_AttributeId").OnTable("CustomerAttributeLinks");
            Delete.ForeignKey("FK_CustomerAttributeLinks_CustomerId").OnTable("CustomerAttributeLinks");
            Delete.PrimaryKey("PK_CustomerAttributeLinks_Id").FromTable("CustomerAttributeLinks");
            Delete.Table("CustomerAttributeLinks");
        }
    }
}
