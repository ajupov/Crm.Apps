using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190627001701)]
    public class Migration20190627001701AddTableCompanyBankAccounts : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyBankAccounts")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyBankAccounts_Id")
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("Number").AsString(64).NotNullable()
                .WithColumn("BankNumber").AsString(64).NotNullable()
                .WithColumn("BankCorrespondentNumber").AsString(64).NotNullable()
                .WithColumn("BankName").AsString(256).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_CompanyBankAccounts_CompanyId")
                .FromTable("CompanyBankAccounts").ForeignColumn("CompanyId")
                .ToTable("Companies").PrimaryColumn("Id");

            Create.Index(
                    "IX_CompanyBankAccounts_CompanyId_Number_BankNumber_BankCorrespondentNumber_BankName_CreateDateTime")
                .OnTable("CompanyBankAccounts")
                .OnColumn("CompanyId").Descending()
                .OnColumn("Number").Ascending()
                .OnColumn("BankNumber").Ascending()
                .OnColumn("BankCorrespondentNumber").Ascending()
                .OnColumn("BankName").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_CompanyBankAccounts_CompanyId_Number_BankNumber_BankCorrespondentNumber_BankName_CreateDateTime")
                .OnTable("CompanyBankAccounts");
            Delete.ForeignKey("FK_CompanyBankAccounts_CompanyId").OnTable("CompanyBankAccounts");
            Delete.Table("CompanyBankAccounts");
        }
    }
}