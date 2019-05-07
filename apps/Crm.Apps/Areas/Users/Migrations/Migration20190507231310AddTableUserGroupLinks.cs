using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190507231310)]
    public class Migration20190507231310AddTableUserGroupLinks : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroupLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserGroupLinks_Id")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("GroupId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_UserGroupLinks_UserId")
                .FromTable("UserGroupLinks").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey("FK_UserGroupLinks_GroupId")
                .FromTable("UserGroupLinks").ForeignColumn("GroupId")
                .ToTable("UserAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks")
                .Columns("UserId", "GroupId");

            Create.Index("IX_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks")
                .OnColumn("UserId").Descending()
                .OnColumn("GroupId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupLinks_UserId_GroupId").OnTable("UserGroupLinks");
            Delete.UniqueConstraint("UQ_UserGroupLinks_UserId_GroupId").FromTable("UserGroupLinks");
            Delete.ForeignKey("FK_UserGroupLinks_GroupId").OnTable("UserGroupLinks");
            Delete.ForeignKey("FK_UserGroupLinks_UserId").OnTable("UserGroupLinks");
            Delete.Table("UserGroupLinks");
        }
    }
}