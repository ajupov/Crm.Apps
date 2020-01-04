using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190429233211)]
    public class Migration20190429233211AddTableUserRoles : Migration
    {
        public override void Up()
        {
            Create.Table("UserRoles")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Role").AsByte().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_UserRoles_Id").OnTable("UserRoles")
                .Column("Id");

            Create.ForeignKey("FK_UserRoles_UserId")
                .FromTable("UserRoles").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserRoles_UserId_Role").OnTable("UserRoles")
                .Columns("UserId", "Role");

            Create.Index("IX_UserRoles_UserId").OnTable("UserRoles")
                .OnColumn("UserId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserRoles_UserId").OnTable("UserRoles");
            Delete.UniqueConstraint("UQ_UserRoles_UserId_Role").FromTable("UserRoles");
            Delete.ForeignKey("FK_UserRoles_GroupId").OnTable("UserRoles");
            Delete.PrimaryKey("PK_UserRoles_Id").FromTable("UserRoles");
            Delete.Table("UserRoles");
        }
    }
}