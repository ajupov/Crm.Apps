using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712213634)]
    public class Migration20190712213634AddTableActivityComments : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityComments")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityComments_Id")
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_ActivityComments_ActivityId_CommentatorUserId_Value_CreateDateTime").OnTable("ActivityComments")
                .OnColumn("ActivityId").Descending()
                .OnColumn("CommentatorUserId").Descending()
                .OnColumn("Value").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityComments_ActivityId_CommentatorUserId_Value_CreateDateTime").OnTable("ActivityComments");
            Delete.Table("ActivityComments");
        }
    }
}