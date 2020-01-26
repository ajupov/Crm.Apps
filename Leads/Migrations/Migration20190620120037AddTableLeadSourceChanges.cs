using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620120037)]
    public class Migration20190620120037AddTableLeadSourceChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadSourceChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_LeadSourceChanges_Id").OnTable("LeadSourceChanges")
                .Column("Id");

            Create.Index("IX_LeadSourceChanges_SourceId_CreateDateTime").OnTable("LeadSourceChanges")
                .OnColumn("SourceId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadSourceChanges_SourceId_CreateDateTime").OnTable("LeadSourceChanges");
            Delete.PrimaryKey("PK_LeadSourceChanges_Id").FromTable("LeadSourceChanges");
            Delete.Table("LeadSourceChanges");
        }
    }
}