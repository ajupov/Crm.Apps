using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115121)]
    public class Migration20190620115121AddTableLeadAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("LeadAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_LeadAttributes_Id").OnTable("LeadAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_LeadAttributes_AccountId_Key").OnTable("LeadAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_LeadAttributes_AccountId").OnTable("LeadAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadAttributes_AccountId").OnTable("LeadAttributes");
            Delete.UniqueConstraint("UQ_LeadAttributes_AccountId_Key").FromTable("LeadAttributes");
            Delete.PrimaryKey("PK_LeadAttributes_Id").FromTable("LeadAttributes");
            Delete.Table("LeadAttributes");
        }
    }
}