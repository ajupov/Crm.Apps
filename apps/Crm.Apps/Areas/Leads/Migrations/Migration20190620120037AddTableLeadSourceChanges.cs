using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620120037)]
    public class Migration20190620120037AddTableLeadSourceChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadSourceChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadSourceChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_LeadSourceChanges_ChangerUserId_SourceId_CreateDateTime")
                .OnTable("LeadSourceChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("SourceId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadSourceChanges_ChangerUserId_SourceId_CreateDateTime")
                .OnTable("LeadSourceChanges");
            Delete.Table("LeadSourceChanges");
        }
    }
}