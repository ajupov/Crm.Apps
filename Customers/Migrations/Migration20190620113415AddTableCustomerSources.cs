using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620113415)]
    public class Migration20190620113415AddTableCustomerSources : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerSources")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_CustomerSources_Id").OnTable("CustomerSources")
                .Column("Id");

            Create.UniqueConstraint("UQ_CustomerSources_AccountId_Name").OnTable("CustomerSources")
                .Columns("AccountId", "Name");

            Create.Index("IX_CustomerSources_AccountId").OnTable("CustomerSources")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerSources_AccountId").OnTable("CustomerSources");
            Delete.UniqueConstraint("UQ_CustomerSources_AccountId_Name").FromTable("CustomerSources");
            Delete.PrimaryKey("PK_CustomerSources_Id").FromTable("CustomerSources");
            Delete.Table("CustomerSources");
        }
    }
}
