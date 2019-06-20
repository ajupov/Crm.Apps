using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620113415)]
    public class Migration20190620113415AddTableLeadSources : Migration
    {
        public override void Up()
        {
            Create.Table("LeadSources")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadSources_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();
            
            Create.UniqueConstraint("UQ_LeadSources_AccountId_Name").OnTable("LeadSources")
                .Columns("AccountId", "Name");
            
            Create.Index("IX_LeadSources_AccountId_Name_IsDeleted_CreateDateTime")
                .OnTable("LeadSources")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadSources_AccountId_Name_IsDeleted_CreateDateTime").OnTable("LeadSources");
            Delete.UniqueConstraint("UQ_LeadSources_AccountId_Name").FromTable("LeadSources");
            Delete.Table("LeadSources");
        }
    }
}