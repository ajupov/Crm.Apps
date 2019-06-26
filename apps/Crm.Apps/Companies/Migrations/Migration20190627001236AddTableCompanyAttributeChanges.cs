using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190627001236)]
    public class Migration20190627001236AddTableCompanyAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_CompanyAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("CompanyAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("CompanyAttributeChanges");
            Delete.Table("CompanyAttributeChanges");
        }
    }
}