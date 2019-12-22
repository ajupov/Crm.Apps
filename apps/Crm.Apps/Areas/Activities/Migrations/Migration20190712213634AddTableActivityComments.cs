using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712213634)]
    public class Migration20190712213634AddTableActivityComments : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_ActivityComments_Id").OnTable("ActivityComments")
                .Columns("Id");

            Create.Index("IX_ActivityComments_ActivityId_CreateDateTime").OnTable("ActivityComments")
                .OnColumn("ActivityId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityComments_ActivityId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("ActivityComments");
            Delete.PrimaryKey("PK_ActivityComments_Id").FromTable("ActivityComments");
            Delete.Table("ActivityComments");
        }
    }
}