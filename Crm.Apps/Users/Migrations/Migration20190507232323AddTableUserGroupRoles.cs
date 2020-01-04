using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507232323)]
    public class Migration20190507232323AddTableUserGroupRoles : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupRoles")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("Role").AsByte().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_UserGroupRoles_Id").OnTable("UserGroupRoles")
                .Column("Id");

            Create.ForeignKey("FK_UserGroupRoles_GroupId")
                .FromTable("UserGroupRoles").ForeignColumn("GroupId")
                .ToTable("UserGroups").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserGroupRoles_GroupId_Role").OnTable("UserGroupRoles")
                .Columns("GroupId", "Role");

            Create.Index("IX_UserGroupRoles_GroupId").OnTable("UserGroupRoles")
                .OnColumn("GroupId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupRoles_GroupId").OnTable("UserGroupRoles");
            Delete.UniqueConstraint("UQ_UserGroupRoles_GroupId_Role").FromTable("UserGroupRoles");
            Delete.ForeignKey("FK_UserGroupRoles_GroupId").OnTable("UserGroupRoles");
            Delete.PrimaryKey("PK_UserGroupRoles_Id").FromTable("UserGroupRoles");
            Delete.Table("UserGroupRoles");
        }
    }
}