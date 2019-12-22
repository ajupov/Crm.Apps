using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190627001236)]
    public class Migration20190627001236AddTableCompanyAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.PrimaryKey("PK_CompanyAttributeChanges_Id").OnTable("CompanyAttributeChanges")
                .Column("Id");

            Create.Index("IX_CompanyAttributeChanges_AttributeId_CreateDateTime")
                .OnTable("CompanyAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyAttributeChanges_AttributeId_CreateDateTime").OnTable("CompanyAttributeChanges");
            Delete.PrimaryKey("PK_CompanyAttributeChanges_Id").FromTable("CompanyAttributeChanges");
            Delete.Table("CompanyAttributeChanges");
        }
    }
}