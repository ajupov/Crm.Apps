using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712211717)]
    public class Migration20190712211717AddTableActivityAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().Nullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ActivityAttributeLinks_Id").OnTable("ActivityAttributeLinks")
                .Columns("Id");

            Create.ForeignKey("FK_ActivityAttributeLinks_ActivityId")
                .FromTable("ActivityAttributeLinks").ForeignColumn("ActivityId")
                .ToTable("Activities").PrimaryColumn("Id");

            Create.ForeignKey("FK_ActivityAttributeLinks_AttributeId")
                .FromTable("ActivityAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("ActivityAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ActivityAttributeLinks_ActivityId_AttributeId")
                .OnTable("ActivityAttributeLinks")
                .Columns("ActivityId", "AttributeId");

            Create.Index("IX_ActivityAttributeLinks_ActivityId_AttributeId").OnTable("ActivityAttributeLinks")
                .OnColumn("ActivityId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityAttributeLinks_ActivityId_AttributeId").OnTable("ActivityAttributeLinks");
            Delete.UniqueConstraint("UQ_ActivityAttributeLinks_ActivityId_AttributeId")
                .FromTable("ActivityAttributeLinks");
            Delete.ForeignKey("FK_ActivityAttributeLinks_AttributeId").OnTable("ActivityAttributeLinks");
            Delete.ForeignKey("FK_ActivityAttributeLinks_ActivityId").OnTable("ActivityAttributeLinks");
            Delete.PrimaryKey("PK_ActivityAttributeLinks_Id").FromTable("ActivityAttributeLinks");
            Delete.Table("ActivityAttributeLinks");
        }
    }
}