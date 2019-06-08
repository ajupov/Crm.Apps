using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507224536)]
    public class Migration20190507224536AddTableUserAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_UserAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("UserAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("UserAttributeChanges");
            Delete.Table("UserAttributeChanges");
        }
    }
}