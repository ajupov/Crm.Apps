using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190429233211)]
    public class Migration20190429233211AddTableUserPermissions : Migration
    {
        public override void Up()
        {
            Create.Table("UserPermissions")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserPermissions_Id")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Permission").AsByte().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_UserPermissions_UserId")
                .FromTable("UserPermissions").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserPermissions_UserId_Permission").OnTable("UserPermissions")
                .Columns("UserId", "Permission");

            Create.Index("IX_UserPermissions_UserId_Permission_CreateDateTime").OnTable("UserPermissions")
                .OnColumn("UserId").Descending()
                .OnColumn("Permission").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserPermissions_UserId_Permission_CreateDateTime").OnTable("UserPermissions");
            Delete.UniqueConstraint("UQ_UserPermissions_UserId_Permission").FromTable("UserPermissions");
            Delete.ForeignKey("FK_UserPermissions_GroupId").OnTable("UserPermissions");
            Delete.Table("UserPermissions");
        }
    }
}