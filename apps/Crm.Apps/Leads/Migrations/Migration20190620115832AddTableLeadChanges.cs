using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115832)]
    public class Migration20190620115832AddTableLeadChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_LeadChanges_Id").OnTable("LeadChanges")
                .Column("Id");

            Create.Index("IX_LeadChanges_LeadId_CreateDateTime").OnTable("LeadChanges")
                .OnColumn("LeadId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadChanges_LeadId_CreateDateTime").OnTable("LeadChanges");
            Delete.PrimaryKey("PK_LeadChanges_Id").FromTable("LeadChanges");
            Delete.Table("LeadChanges");
        }
    }
}