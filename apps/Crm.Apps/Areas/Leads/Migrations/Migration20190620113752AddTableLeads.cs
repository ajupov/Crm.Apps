using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620113752)]
    public class Migration20190620113752AddTableLeads : Migration
    {
        public override void Up()
        {
            Create.Table("Leads")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(64).NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("Patronymic").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("CompanyName").AsString(256).NotNullable()
                .WithColumn("Post").AsString(256).NotNullable()
                .WithColumn("Postcode").AsString(8).NotNullable()
                .WithColumn("Country").AsString(64).NotNullable()
                .WithColumn("Region").AsString(64).NotNullable()
                .WithColumn("Province").AsString(64).NotNullable()
                .WithColumn("City").AsString(64).NotNullable()
                .WithColumn("Street").AsString(64).NotNullable()
                .WithColumn("House").AsString(64).NotNullable()
                .WithColumn("Apartment").AsString(64).NotNullable()
                .WithColumn("OpportunitySum").AsDecimal(18, 2).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Leads_Id").OnTable("Leads")
                .Column("Id");

            Create.ForeignKey("FK_Leads_SourceId")
                .FromTable("Leads").ForeignColumn("SourceId")
                .ToTable("LeadSources").PrimaryColumn("Id");

            Create.Index("IX_Leads_AccountId_SourceId_CreateUserId_ResponsibleUserId")
                .OnTable("Leads")
                .OnColumn("AccountId").Ascending()
                .OnColumn("SourceId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Leads_AccountId_SourceId_CreateUserId_ResponsibleUserId").OnTable("Leads");
            Delete.UniqueConstraint("UQ_Leads_AccountId_Name").FromTable("Leads");
            Delete.ForeignKey("FK_Leads_SourceId").OnTable("Leads");
            Delete.PrimaryKey("PK_Leads_Id").FromTable("Leads");
            Delete.Table("Leads");
        }
    }
}