using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190627001510)]
    public class Migration20190627001510AddTableCompanyComments : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyComments")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyComments_Id")
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_CompanyComments_CompanyId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("CompanyComments")
                .OnColumn("CompanyId").Descending()
                .OnColumn("CommentatorUserId").Descending()
                .OnColumn("Value").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyComments_CompanyId_CommentatorUserId_Value_CreateDateTime")
                .OnTable("CompanyComments");
            Delete.Table("CompanyComments");
        }
    }
}