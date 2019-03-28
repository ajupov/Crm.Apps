using FluentMigrator;

namespace Crm.Apps.Areas.Accounts.Migrations
{
    [Migration(20190313005044)]
    public class Migration20190313005044AddTableAccountChanges : Migration
    {
        public override void Up()
        {
            Create.Table("AccountChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_AccountChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("DateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.ForeignKey("FK_AccountChanges_AccountId")
                .FromTable("AccountChanges").ForeignColumn("AccountId")
                .ToTable("Accounts").PrimaryColumn("Id");

            Create.Index("IX_AccountChanges_ChangerUserId_AccountId").OnTable("AccountChanges")
                .OnColumn("ChangerUserId").Ascending()
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountChanges_ChangerUserId_AccountId").OnTable("AccountChanges");
            Delete.ForeignKey("FK_AccountChanges_AccountId");
            Delete.Table("AccountChanges");
        }
    }
}