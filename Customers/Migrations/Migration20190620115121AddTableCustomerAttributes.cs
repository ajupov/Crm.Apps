using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620115121)]
    public class Migration20190620115121AddTableCustomerAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_CustomerAttributes_Id").OnTable("CustomerAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_CustomerAttributes_AccountId_Key").OnTable("CustomerAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_CustomerAttributes_AccountId").OnTable("CustomerAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerAttributes_AccountId").OnTable("CustomerAttributes");
            Delete.UniqueConstraint("UQ_CustomerAttributes_AccountId_Key").FromTable("CustomerAttributes");
            Delete.PrimaryKey("PK_CustomerAttributes_Id").FromTable("CustomerAttributes");
            Delete.Table("CustomerAttributes");
        }
    }
}
