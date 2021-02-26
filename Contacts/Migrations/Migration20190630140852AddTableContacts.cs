using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630140852)]
    public class Migration20190630140852AddTableContacts : Migration
    {
        public override void Up()
        {
            Create.Table("Contacts")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().Nullable()
                .WithColumn("CompanyId").AsGuid().Nullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("Surname").AsString(64).NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("Patronymic").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("TaxNumber").AsString(64).NotNullable()
                .WithColumn("Post").AsString(256).NotNullable()
                .WithColumn("Postcode").AsString(8).NotNullable()
                .WithColumn("Country").AsString(64).NotNullable()
                .WithColumn("Region").AsString(64).NotNullable()
                .WithColumn("Province").AsString(64).NotNullable()
                .WithColumn("City").AsString(64).NotNullable()
                .WithColumn("Street").AsString(64).NotNullable()
                .WithColumn("House").AsString(64).NotNullable()
                .WithColumn("Apartment").AsString(64).NotNullable()
                .WithColumn("BirthDate").AsDate().Nullable()
                .WithColumn("Photo").AsBinary().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Contacts_Id").OnTable("Contacts")
                .Column("Id");

            Create.Index("IX_Contacts_AccountId_LeadId_CompanyId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Contacts")
                .OnColumn("AccountId").Ascending()
                .OnColumn("LeadId").Ascending()
                .OnColumn("CompanyId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Contacts_AccountId_LeadId_CompanyId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Contacts");
            Delete.PrimaryKey("PK_Contacts_Id").FromTable("Contacts");
            Delete.Table("Contacts");
        }
    }
}
