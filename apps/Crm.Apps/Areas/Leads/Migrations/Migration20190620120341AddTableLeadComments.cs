using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620120341)]
    public class Migration20190620120341AddTableLeadComments : Migration
    {
        public override void Up()
        {
            Create.Table("LeadComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_LeadComments_Id").OnTable("LeadComments")
                .Column("Id");

            Create.Index("IX_LeadComments_LeadId_CreateDateTime").OnTable("LeadComments")
                .OnColumn("LeadId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadComments_LeadId_CreateDateTime").OnTable("LeadComments");
            Delete.PrimaryKey("PK_LeadComments_Id").FromTable("LeadComments");
            Delete.Table("LeadComments");
        }
    }
}