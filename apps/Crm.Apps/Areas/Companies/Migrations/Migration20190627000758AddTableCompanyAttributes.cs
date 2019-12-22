using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190627000758)]
    public class Migration20190627000758AddTableCompanyAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_CompanyAttributes_AccountId_Key").OnTable("CompanyAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_CompanyAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("CompanyAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("CompanyAttributes");
            Delete.UniqueConstraint("UQ_CompanyAttributes_AccountId_Key").FromTable("CompanyAttributes");
            Delete.Table("CompanyAttributes");
        }
    }
}