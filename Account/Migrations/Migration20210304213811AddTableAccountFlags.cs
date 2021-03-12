using FluentMigrator;

namespace Crm.Apps.Account.Migrations
{
    [Migration(20210304213811)]
    public class Migration20210304213811AddTableAccountFlags : Migration
    {
        public override void Up()
        {
            Create.Table("AccountFlags")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("SetDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_AccountFlags_Id").OnTable("AccountFlags")
                .Column("Id");

            Create.UniqueConstraint("UQ_AccountFlags_AccountId_Type").OnTable("AccountFlags")
                .Columns("AccountId", "Type");

            Create.Index("IX_AccountFlags_AccountId").OnTable("AccountFlags")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountFlags_AccountId").OnTable("AccountFlags");
            Delete.UniqueConstraint("UQ_AccountFlags_AccountId_Type").FromTable("AccountFlags");
            Delete.PrimaryKey("PK_AccountFlags_Id").FromTable("AccountFlags");
            Delete.Table("AccountFlags");
        }
    }
}
