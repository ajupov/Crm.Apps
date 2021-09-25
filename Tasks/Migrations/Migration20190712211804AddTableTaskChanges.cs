using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712211804)]
    public class Migration20190712211804AddTableTaskChanges : Migration
    {
        public override void Up()
        {
            Create.Table("TaskChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TaskId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_TaskChanges_Id").OnTable("TaskChanges")
                .Columns("Id");

            Create.Index("IX_TaskChanges_TaskId_CreateDateTime").OnTable("TaskChanges")
                .OnColumn("TaskId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskChanges_TaskId_CreateDateTime").OnTable("TaskChanges");
            Delete.PrimaryKey("PK_TaskChanges_Id").FromTable("TaskChanges");
            Delete.Table("TaskChanges");
        }
    }
}
