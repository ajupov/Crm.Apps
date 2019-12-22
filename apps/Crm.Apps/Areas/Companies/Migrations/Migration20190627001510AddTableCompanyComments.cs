using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190627001510)]
    public class Migration20190627001510AddTableCompanyComments : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_CompanyComments_Id").OnTable("CompanyComments")
                .Column("Id");

            Create.Index("IX_CompanyComments_CompanyId_CreateDateTime").OnTable("CompanyComments")
                .OnColumn("CompanyId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyComments_CompanyId_CreateDateTime").OnTable("CompanyComments");
            Delete.PrimaryKey("PK_CompanyComments_Id").FromTable("CompanyComments");
            Delete.Table("CompanyComments");
        }
    }
}