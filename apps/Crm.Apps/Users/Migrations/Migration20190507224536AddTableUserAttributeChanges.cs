using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507224536)]
    public class Migration20190507224536AddTableUserAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_UserAttributeChanges_Id").OnTable("UserAttributeChanges")
                .Column("Id");

            Create.Index("IX_UserAttributeChanges_AttributeId").OnTable("UserAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserAttributeChanges_AttributeId").OnTable("UserAttributeChanges");
            Delete.PrimaryKey("PK_UserAttributeChanges_Id").FromTable("UserAttributeChanges");
            Delete.Table("UserAttributeChanges");
        }
    }
}