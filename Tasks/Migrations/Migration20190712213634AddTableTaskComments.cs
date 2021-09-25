using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712213634)]
    public class Migration20190712213634AddTableTaskComments : Migration
    {
        public override void Up()
        {
            Create.Table("TaskComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("TaskId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_TaskComments_Id").OnTable("TaskComments")
                .Columns("Id");

            Create.Index("IX_TaskComments_TaskId_CreateDateTime").OnTable("TaskComments")
                .OnColumn("TaskId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskComments_TaskId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("TaskComments");
            Delete.PrimaryKey("PK_TaskComments_Id").FromTable("TaskComments");
            Delete.Table("TaskComments");
        }
    }
}
