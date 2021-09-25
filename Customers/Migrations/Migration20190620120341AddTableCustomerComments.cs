using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620120341)]
    public class Migration20190620120341AddTableCustomerComments : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("CustomerId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_CustomerComments_Id").OnTable("CustomerComments")
                .Column("Id");

            Create.Index("IX_CustomerComments_CustomerId_CreateDateTime").OnTable("CustomerComments")
                .OnColumn("CustomerId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerComments_CustomerId_CreateDateTime").OnTable("CustomerComments");
            Delete.PrimaryKey("PK_CustomerComments_Id").FromTable("CustomerComments");
            Delete.Table("CustomerComments");
        }
    }
}
