using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190507224536)]
    public class Migration20190507224536AddTableUserAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserAttributesChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserAttributesChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_UserAttributesChanges_ChangerUserId_AttributeId").OnTable("UserAttributesChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserAttributesChanges_ChangerUserId_AttributeId").OnTable("UserAttributesChanges");
            Delete.Table("UserAttributesChanges");
        }
    }
}