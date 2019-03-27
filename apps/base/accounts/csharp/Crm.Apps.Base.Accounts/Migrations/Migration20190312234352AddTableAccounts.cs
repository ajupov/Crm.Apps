using FluentMigrator;

namespace Crm.Apps.Base.Accounts.Migrations
{
    [Migration(20190312234352)]
    public class Migration20190312234352AddTableAccounts : Migration
    {
        public override void Up()
        {
            Create.Table("Accounts")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Accounts_Id")
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_Accounts_CreateDateTime_IsLocked_IsDeleted").OnTable("Accounts")
                .OnColumn("CreateDateTime").Descending()
                .OnColumn("IsLocked").Descending()
                .OnColumn("IsDeleted").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Accounts_CreateDateTime_IsLocked_IsDeleted").OnTable("Accounts");

            Delete.Table("Accounts");
        }
    }
}