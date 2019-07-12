using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211739)]
    public class Migration20190712211739AddTableActivityAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ActivityAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("ActivityAttributeChanges");
            Delete.Table("ActivityAttributeChanges");
        }
    }
}