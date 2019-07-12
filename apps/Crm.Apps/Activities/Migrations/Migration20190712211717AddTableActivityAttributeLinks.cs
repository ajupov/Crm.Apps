using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211717)]
    public class Migration20190712211717AddTableActivityAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityAttributeLinks_Id")
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_ActivityAttributeLinks_ActivityId")
                .FromTable("ActivityAttributeLinks").ForeignColumn("ActivityId")
                .ToTable("Activities").PrimaryColumn("Id");

            Create.ForeignKey("FK_ActivityAttributeLinks_AttributeId")
                .FromTable("ActivityAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("ActivityAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ActivityAttributeLinks_ActivityId_AttributeId")
                .OnTable("ActivityAttributeLinks")
                .Columns("ActivityId", "AttributeId");

            Create.Index("IX_ActivityAttributeLinks_ActivityId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeLinks")
                .OnColumn("ActivityId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityAttributeLinks_ActivityId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeLinks");
            Delete.UniqueConstraint("UQ_ActivityAttributeLinks_ActivityId_AttributeId")
                .FromTable("ActivityAttributeLinks");
            Delete.ForeignKey("FK_ActivityAttributeLinks_AttributeId").OnTable("ActivityAttributeLinks");
            Delete.ForeignKey("FK_ActivityAttributeLinks_ActivityId").OnTable("ActivityAttributeLinks");
            Delete.Table("ActivityAttributeLinks");
        }
    }
}