using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620115658)]
    public class Migration20190620115658AddTableCustomerAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_CustomerAttributeChanges_Id").OnTable("CustomerAttributeChanges")
                .Column("Id");

            Create.Index("IX_CustomerAttributeChanges_AttributeId_CreateDateTime").OnTable("CustomerAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerAttributeChanges_AttributeId_CreateDateTime").OnTable("CustomerAttributeChanges");
            Delete.PrimaryKey("PK_CustomerAttributeChanges_Id").FromTable("CustomerAttributeChanges");
            Delete.Table("CustomerAttributeChanges");
        }
    }
}
