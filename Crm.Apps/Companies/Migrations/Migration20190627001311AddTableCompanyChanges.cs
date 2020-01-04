using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190627001311)]
    public class Migration20190627001311AddTableCompanyChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_CompanyChanges_Id").OnTable("CompanyChanges")
                .Column("Id");

            Create.Index("IX_CompanyChanges_CompanyId_CreateDateTime").OnTable("CompanyChanges")
                .OnColumn("CompanyId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyChanges_CompanyId_CreateDateTime").OnTable("CompanyChanges");
            Delete.PrimaryKey("PK_CompanyChanges_Id").FromTable("CompanyChanges");
            Delete.Table("CompanyChanges");
        }
    }
}