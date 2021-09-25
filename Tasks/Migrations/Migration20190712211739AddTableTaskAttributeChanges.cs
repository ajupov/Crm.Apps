using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712211739)]
    public class Migration20190712211739AddTableTaskAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("TaskAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_TaskAttributeChanges_Id").OnTable("TaskAttributeChanges")
                .Columns("Id");

            Create.Index("IX_TaskAttributeChanges_AttributeId_CreateDateTime").OnTable("TaskAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskAttributeChanges_AttributeId_CreateDateTime").OnTable("TaskAttributeChanges");
            Delete.PrimaryKey("PK_TaskAttributeChanges_Id").FromTable("TaskAttributeChanges");
            Delete.Table("TaskAttributeChanges");
        }
    }
}
