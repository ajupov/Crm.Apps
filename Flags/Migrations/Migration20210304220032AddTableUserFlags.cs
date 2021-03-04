using FluentMigrator;

namespace Crm.Apps.Flags.Migrations
{
    [Migration(20210304220032)]
    public class Migration20210304220032AddTableUserFlags : Migration
    {
        public override void Up()
        {
            Create.Table("UserFlags")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("SetDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_UserFlags_Id").OnTable("UserFlags")
                .Column("Id");

            Create.UniqueConstraint("UQ_UserFlags_UserId_Type").OnTable("UserFlags")
                .Columns("UserId", "Type");

            Create.Index("IX_UserFlags_UserId").OnTable("UserFlags")
                .OnColumn("UserId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserFlags_UserId").OnTable("UserFlags");
            Delete.UniqueConstraint("UQ_UserFlags_UserId_Type").FromTable("UserFlags");
            Delete.PrimaryKey("PK_UserFlags_Id").FromTable("UserFlags");
            Delete.Table("UserFlags");
        }
    }
}
