using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630141241)]
    public class Migration20190630141241AddTableContactComments : Migration
    {
        public override void Up()
        {
            Create.Table("ContactComments")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactComments_Id")
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_ContactComments_ContactId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("ContactComments")
                .OnColumn("ContactId").Descending()
                .OnColumn("CommentatorUserId").Descending()
                .OnColumn("Value").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactComments_ContactId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("ContactComments");
            Delete.Table("ContactComments");
        }
    }
}