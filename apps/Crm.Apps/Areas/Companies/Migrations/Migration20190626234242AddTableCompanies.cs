using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190626234242)]
    public class Migration20190626234242AddTableCompanies : Migration
    {
        public override void Up()
        {
            Create.Table("Companies")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("IndustryType").AsByte().NotNullable()
                .WithColumn("FullName").AsString(256).NotNullable()
                .WithColumn("ShortName").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("TaxNumber").AsString(64).NotNullable()
                .WithColumn("RegistrationNumber").AsString(64).NotNullable()
                .WithColumn("RegistrationDate").AsDateTime2().Nullable()
                .WithColumn("EmployeesCount").AsInt32().NotNullable()
                .WithColumn("YearlyTurnover").AsDecimal(18, 2).NotNullable()
                .WithColumn("JuridicalPostcode").AsString(8).NotNullable()
                .WithColumn("JuridicalCountry").AsString(64).NotNullable()
                .WithColumn("JuridicalRegion").AsString(64).NotNullable()
                .WithColumn("JuridicalProvince").AsString(64).NotNullable()
                .WithColumn("JuridicalCity").AsString(64).NotNullable()
                .WithColumn("JuridicalStreet").AsString(64).NotNullable()
                .WithColumn("JuridicalHouse").AsString(64).NotNullable()
                .WithColumn("JuridicalApartment").AsString(64).NotNullable()
                .WithColumn("LegalPostcode").AsString(8).NotNullable()
                .WithColumn("LegalCountry").AsString(64).NotNullable()
                .WithColumn("LegalRegion").AsString(64).NotNullable()
                .WithColumn("LegalProvince").AsString(64).NotNullable()
                .WithColumn("LegalCity").AsString(64).NotNullable()
                .WithColumn("LegalStreet").AsString(64).NotNullable()
                .WithColumn("LegalHouse").AsString(64).NotNullable()
                .WithColumn("LegalApartment").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Companies_Id").OnTable("Companies")
                .Column("Id");

            Create.UniqueConstraint("UQ_Companies_TaxNumber").OnTable("Companies")
                .Columns("AccountId", "TaxNumber");

            Create.Index("IX_Companies_AccountId_LeadId_CreateUserId_ResponsibleUserId")
                .OnTable("Companies")
                .OnColumn("AccountId").Ascending()
                .OnColumn("LeadId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Companies_AccountId_LeadId_CreateUserId_ResponsibleUserId").OnTable("Companies");
            Delete.UniqueConstraint("UQ_Companies_TaxNumber").FromTable("Companies");
            Delete.PrimaryKey("PK_Companies_Id").FromTable("Companies");
            Delete.Table("Companies");
        }
    }
}