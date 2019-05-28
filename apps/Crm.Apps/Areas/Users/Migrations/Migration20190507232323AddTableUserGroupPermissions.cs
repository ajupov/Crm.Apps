using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190507232323)]
    public class Migration20190507232323AddTableUserGroupPermissions : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupPermissions")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserGroupPermissions_Id")
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("Permission").AsByte().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_UserGroupPermissions_GroupId")
                .FromTable("UserGroupPermissions").ForeignColumn("GroupId")
                .ToTable("UserGroups").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserGroupPermissions_GroupId_Permission").OnTable("UserGroupPermissions")
                .Columns("GroupId", "Permission");

            Create.Index("IX_UserGroupPermissions_GroupId_Permission_CreateDateTime").OnTable("UserGroupPermissions")
                .OnColumn("GroupId").Descending()
                .OnColumn("Permission").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupPermissions_GroupId_Permission_CreateDateTime").OnTable("UserGroupPermissions");
            Delete.UniqueConstraint("UQ_UserGroupPermissions_GroupId_Permission").FromTable("UserGroupPermissions");
            Delete.ForeignKey("FK_UserGroupPermissions_GroupId").OnTable("UserGroupPermissions");
            Delete.Table("UserGroupPermissions");
        }
    }
}