using FluentMigrator;

namespace Crm.Apps.Areas.Accounts.Migrations
{
    [Migration(20190312234352)]
    public class Migration20190312234352AddTableAccounts : Migration
    {
        public override void Up()
        {
            Create.Table("Accounts")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Accounts_Id").OnTable("Accounts")
                .Column("Id");

            Create.Index("IX_Accounts_CreateDateTime").OnTable("Accounts")
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Accounts_CreateDateTime").OnTable("Users");
            Delete.PrimaryKey("PK_Accounts_Id").FromTable("Accounts");
            Delete.Table("Accounts");
        }
    }
}