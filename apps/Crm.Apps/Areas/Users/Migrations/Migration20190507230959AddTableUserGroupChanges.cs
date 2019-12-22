using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190507230959)]
    public class Migration20190507230959AddTableUserGroupChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_UserGroupChanges_Id").OnTable("UserGroupChanges")
                .Column("Id");

            Create.Index("IX_UserGroupChanges_GroupId").OnTable("UserGroupChanges")
                .OnColumn("GroupId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupChanges_GroupId").OnTable("UserGroupChanges");
            Delete.PrimaryKey("PK_UserGroupChanges_Id").FromTable("UserGroupChanges");
            Delete.Table("UserGroupChanges");
        }
    }
}