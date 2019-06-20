using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115832)]
    public class Migration20190620115832AddTableLeadChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_LeadChanges_ChangerUserId_LeadId_CreateDateTime").OnTable("LeadChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("LeadId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadChanges_ChangerUserId_LeadId_CreateDateTime").OnTable("LeadChanges");
            Delete.Table("LeadChanges");
        }
    }
}