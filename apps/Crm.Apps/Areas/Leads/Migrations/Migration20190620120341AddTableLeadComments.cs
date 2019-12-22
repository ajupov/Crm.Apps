using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620120341)]
    public class Migration20190620120341AddTableLeadComments : Migration
    {
        public override void Up()
        {
            Create.Table("LeadComments")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadComments_Id")
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_LeadComments_LeadId_CommentatorUserId_Value_CreateDateTime").OnTable("LeadComments")
                .OnColumn("LeadId").Descending()
                .OnColumn("CommentatorUserId").Descending()
                .OnColumn("Value").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadComments_LeadId_CommentatorUserId_Value_CreateDateTime").OnTable("LeadComments");
            Delete.Table("LeadComments");
        }
    }
}