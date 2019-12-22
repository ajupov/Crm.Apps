using FluentMigrator;

namespace Crm.Apps.Areas.Contacts.Migrations
{
    [Migration(20190630141241)]
    public class Migration20190630141241AddTableContactComments : Migration
    {
        public override void Up()
        {
            Create.Table("ContactComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_ContactComments_Id").OnTable("ContactComments")
                .Column("Id");

            Create.Index("IX_ContactComments_ContactId_CreateDateTime").OnTable("ContactComments")
                .OnColumn("ContactId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactComments_ContactId_CreateDateTime").OnTable("ContactComments");
            Delete.PrimaryKey("PK_ContactComments_Id").FromTable("ContactComments");
            Delete.Table("ContactComments");
        }
    }
}