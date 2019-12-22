using FluentMigrator;

namespace Crm.Apps.Areas.Accounts.Migrations
{
    [Migration(20190313005044)]
    public class Migration20190313005044AddTableAccountChanges : Migration
    {
        public override void Up()
        {
            Create.Table("AccountChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_AccountChanges_Id").OnTable("AccountChanges")
                .Column("Id");

            Create.Index("IX_AccountChanges_AccountId_CreateDateTime").OnTable("AccountChanges")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountChanges_AccountId_CreateDateTime").OnTable("AccountChanges");
            Delete.PrimaryKey("PK_AccountChanges_Id").FromTable("AccountChanges");
            Delete.Table("AccountChanges");
        }
    }
}