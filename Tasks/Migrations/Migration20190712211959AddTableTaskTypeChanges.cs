using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712211959)]
    public class Migration20190712211959AddTableTaskTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("TaskTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_TaskTypeChanges_Id").OnTable("TaskTypeChanges")
                .Columns("Id");

            Create.Index("IX_TaskTypeChanges_TypeId_CreateDateTime").OnTable("TaskTypeChanges")
                .OnColumn("TypeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskTypeChanges_TypeId_CreateDateTime").OnTable("TaskTypeChanges");
            Delete.PrimaryKey("PK_TaskTypeChanges_Id").FromTable("TaskTypeChanges");
            Delete.Table("TaskTypeChanges");
        }
    }
}
