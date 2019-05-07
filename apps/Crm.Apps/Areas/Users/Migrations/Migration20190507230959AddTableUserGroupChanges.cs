using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
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

            Create.ForeignKey("FK_UserGroupChanges_GroupId")
                .FromTable("UserGroupChanges").ForeignColumn("GroupId")
                .ToTable("UserGroups").PrimaryColumn("Id");

            Create.Index("IX_UserGroupChanges_ChangerUserId_GroupId").OnTable("UserGroupChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("GroupId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroupChanges_ChangerUserId_GroupId").OnTable("UserGroupChanges");
            Delete.ForeignKey("FK_UserGroupChanges_GroupId").OnTable("UserGroupChanges");
            Delete.Table("UserGroupChanges");
        }
    }
}