using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620113415)]
    public class Migration20190620113415AddTableLeadSources : Migration
    {
        public override void Up()
        {
            Create.Table("LeadSources")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_LeadSources_Id").OnTable("LeadSources")
                .Column("Id");

            Create.UniqueConstraint("UQ_LeadSources_AccountId_Name").OnTable("LeadSources")
                .Columns("AccountId", "Name");

            Create.Index("IX_LeadSources_AccountId").OnTable("LeadSources")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadSources_AccountId").OnTable("LeadSources");
            Delete.UniqueConstraint("UQ_LeadSources_AccountId_Name").FromTable("LeadSources");
            Delete.PrimaryKey("PK_LeadSources_Id").FromTable("LeadSources");
            Delete.Table("LeadSources");
        }
    }
}