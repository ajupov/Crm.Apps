using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620113752)]
    public class Migration20190620113752AddTableCustomers : Migration
    {
        public override void Up()
        {
            Create.Table("Customers")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().Nullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("Surname").AsString(64).Nullable()
                .WithColumn("Name").AsString(64).Nullable()
                .WithColumn("Patronymic").AsString(64).Nullable()
                .WithColumn("Phone").AsString(10).Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("BirthDate").AsDate().Nullable()
                .WithColumn("Image").AsString().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Customers_Id").OnTable("Customers")
                .Column("Id");

            Create.ForeignKey("FK_Customers_SourceId")
                .FromTable("Customers").ForeignColumn("SourceId")
                .ToTable("CustomerSources").PrimaryColumn("Id");

            Create.Index("IX_Customers_AccountId_SourceId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Customers")
                .OnColumn("AccountId").Ascending()
                .OnColumn("SourceId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Customers_AccountId_SourceId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Customers");
            Delete.ForeignKey("FK_Customers_SourceId").OnTable("Customers");
            Delete.PrimaryKey("PK_Customers_Id").FromTable("Customers");
            Delete.Table("Customers");
        }
    }
}
