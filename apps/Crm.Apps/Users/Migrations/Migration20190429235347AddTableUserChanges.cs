using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190429235347)]
    public class Migration20190429235347AddTableUserChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_UserChanges_ChangerUserId_UserId_CreateDateTime").OnTable("UserChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("UserId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserChanges_ChangerUserId_UserId_CreateDateTime").OnTable("UserChanges");
            Delete.Table("UserChanges");
        }
    }
}