using FluentMigrator;

namespace Crm.Apps.Accounts.Migrations
{
    [Migration(20190313000217)]
    public class Migration20190313000217AddTableAccountSettings : Migration
    {
        public override void Up()
        {
            Create.Table("AccountSettings")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Value").AsString().Nullable();

            Create.PrimaryKey("PK_AccountSettings_AccountId_Type").OnTable("AccountSettings")
                .Columns("AccountId", "Type");

            Create.ForeignKey("FK_AccountSettings_AccountId")
                .FromTable("AccountSettings").ForeignColumn("AccountId")
                .ToTable("Accounts").PrimaryColumn("Id");

            Create.Index("IX_AccountSettings_AccountId").OnTable("AccountSettings")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountSettings_AccountId").OnTable("AccountSettings");
            Delete.ForeignKey("FK_AccountSettings_AccountId").OnTable("AccountSettings");
            Delete.PrimaryKey("PK_AccountSettings_AccountId_Type").FromTable("AccountSettings");
            Delete.Table("AccountSettings");
        }
    }
}