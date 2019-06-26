using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190627001311)]
    public class Migration20190627001311AddTableCompanyChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_CompanyChanges_ChangerUserId_CompanyId_CreateDateTime").OnTable("CompanyChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("CompanyId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyChanges_ChangerUserId_CompanyId_CreateDateTime").OnTable("CompanyChanges");
            Delete.Table("CompanyChanges");
        }
    }
}