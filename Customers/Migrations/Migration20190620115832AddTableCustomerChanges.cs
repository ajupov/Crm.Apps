using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620115832)]
    public class Migration20190620115832AddTableCustomerChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_CustomerChanges_Id").OnTable("CustomerChanges")
                .Column("Id");

            Create.Index("IX_CustomerChanges_CustomerId_CreateDateTime").OnTable("CustomerChanges")
                .OnColumn("CustomerId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerChanges_CustomerId_CreateDateTime").OnTable("CustomerChanges");
            Delete.PrimaryKey("PK_CustomerChanges_Id").FromTable("CustomerChanges");
            Delete.Table("CustomerChanges");
        }
    }
}
