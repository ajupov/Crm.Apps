using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507230959)]
    public class Migration20190507230959AddTableUserGroupChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserGroupChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_UserGroupChanges_ChangerUserId_GroupId_CreateDateTime").OnTable("UserGroupChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("GroupId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupChanges_ChangerUserId_GroupId_CreateDateTime").OnTable("UserGroupChanges");
            Delete.Table("UserGroupChanges");
        }
    }
}