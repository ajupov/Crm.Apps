using FluentMigrator;

namespace Crm.Apps.Tasks.Migrations
{
    [Migration(20190712211717)]
    public class Migration20190712211717AddTableTaskAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("TaskAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("TaskId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().Nullable();

            Create.PrimaryKey("PK_TaskAttributeLinks_Id").OnTable("TaskAttributeLinks")
                .Columns("Id");

            Create.ForeignKey("FK_TaskAttributeLinks_TaskId")
                .FromTable("TaskAttributeLinks").ForeignColumn("TaskId")
                .ToTable("Tasks").PrimaryColumn("Id");

            Create.ForeignKey("FK_TaskAttributeLinks_AttributeId")
                .FromTable("TaskAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("TaskAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_TaskAttributeLinks_TaskId_AttributeId")
                .OnTable("TaskAttributeLinks")
                .Columns("TaskId", "AttributeId");

            Create.Index("IX_TaskAttributeLinks_TaskId_AttributeId").OnTable("TaskAttributeLinks")
                .OnColumn("TaskId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_TaskAttributeLinks_TaskId_AttributeId").OnTable("TaskAttributeLinks");
            Delete.UniqueConstraint("UQ_TaskAttributeLinks_TaskId_AttributeId")
                .FromTable("TaskAttributeLinks");
            Delete.ForeignKey("FK_TaskAttributeLinks_AttributeId").OnTable("TaskAttributeLinks");
            Delete.ForeignKey("FK_TaskAttributeLinks_TaskId").OnTable("TaskAttributeLinks");
            Delete.PrimaryKey("PK_TaskAttributeLinks_Id").FromTable("TaskAttributeLinks");
            Delete.Table("TaskAttributeLinks");
        }
    }
}
