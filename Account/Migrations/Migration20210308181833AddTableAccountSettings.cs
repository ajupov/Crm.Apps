using FluentMigrator;

namespace Crm.Apps.Account.Migrations
{
    [Migration(20210308181833)]
    public class Migration20210308181833AddTableAccountSettings : Migration
    {
        public override void Up()
        {
            Create.Table("AccountSettings")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("ActivityIndustry").AsInt32().Nullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_AccountSettings_Id").OnTable("AccountSettings")
                .Column("Id");

            Create.UniqueConstraint("UQ_AccountSettings_AccountId").OnTable("AccountSettings")
                .Columns("AccountId");

            Create.Index("IX_AccountSettings_AccountId").OnTable("AccountSettings")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountSettings_AccountId").OnTable("AccountSettings");
            Delete.UniqueConstraint("UQ_AccountSettings_AccountId").FromTable("AccountSettings");
            Delete.PrimaryKey("PK_AccountSettings_Id").FromTable("AccountSettings");
            Delete.Table("AccountSettings");
        }
    }
}
