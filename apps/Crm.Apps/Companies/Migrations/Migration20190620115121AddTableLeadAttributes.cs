using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115121)]
    public class Migration20190620115121AddTableLeadAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("LeadAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_LeadAttributes_AccountId_Key").OnTable("LeadAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_LeadAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("LeadAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("LeadAttributes");
            Delete.UniqueConstraint("UQ_LeadAttributes_AccountId_Key").FromTable("LeadAttributes");
            Delete.Table("LeadAttributes");
        }
    }
}