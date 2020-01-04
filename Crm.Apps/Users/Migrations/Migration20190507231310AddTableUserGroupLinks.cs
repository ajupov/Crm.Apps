using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507231310)]
    public class Migration20190507231310AddTableUserGroupLinks : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_UserGroupLinks_Id").OnTable("UserGroupLinks")
                .Column("Id");

            Create.ForeignKey("FK_UserGroupLinks_UserId")
                .FromTable("UserGroupLinks").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey("FK_UserGroupLinks_GroupId")
                .FromTable("UserGroupLinks").ForeignColumn("GroupId")
                .ToTable("UserGroups").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks")
                .Columns("UserId", "GroupId");

            Create.Index("IX_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks")
                .OnColumn("UserId").Ascending()
                .OnColumn("GroupId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks");
            Delete.UniqueConstraint("UQ_UserGroupLinks_UserId_GroupId").FromTable("UserGroupLinks");
            Delete.ForeignKey("FK_UserGroupLinks_GroupId").OnTable("UserGroupLinks");
            Delete.ForeignKey("FK_UserGroupLinks_UserId").OnTable("UserGroupLinks");
            Delete.PrimaryKey("PK_UserGroupLinks_Id").FromTable("UserGroupLinks");
            Delete.Table("UserGroupLinks");
        }
    }
}