using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712211739)]
    public class Migration20190712211739AddTableActivityAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ActivityAttributeChanges_Id").OnTable("ActivityAttributeChanges")
                .Columns("Id");

            Create.Index("IX_ActivityAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeChanges")
                .OnColumn("ChangerUserId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeChanges");
            Delete.PrimaryKey("PK_ActivityAttributeChanges_Id").FromTable("ActivityAttributeChanges");
            Delete.Table("ActivityAttributeChanges");
        }
    }
}