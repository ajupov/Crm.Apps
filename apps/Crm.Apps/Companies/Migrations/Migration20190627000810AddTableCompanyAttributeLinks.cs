using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190627000810)]
    public class Migration20190627000810AddTableCompanyAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_CompanyAttributeLinks_Id")
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_CompanyAttributeLinks_CompanyId")
                .FromTable("CompanyAttributeLinks").ForeignColumn("CompanyId")
                .ToTable("Companies").PrimaryColumn("Id");

            Create.ForeignKey("FK_CompanyAttributeLinks_AttributeId")
                .FromTable("CompanyAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("CompanyAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_CompanyAttributeLinks_CompanyId_AttributeId").OnTable("CompanyAttributeLinks")
                .Columns("CompanyId", "AttributeId");

            Create.Index("IX_CompanyAttributeLinks_CompanyId_AttributeId_CreateDateTime")
                .OnTable("CompanyAttributeLinks")
                .OnColumn("CompanyId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyAttributeLinks_CompanyId_AttributeId_CreateDateTime")
                .OnTable("CompanyAttributeLinks");
            Delete.UniqueConstraint("UQ_CompanyAttributeLinks_CompanyId_AttributeId")
                .FromTable("CompanyAttributeLinks");
            Delete.ForeignKey("FK_CompanyAttributeLinks_AttributeId").OnTable("CompanyAttributeLinks");
            Delete.ForeignKey("FK_CompanyAttributeLinks_CompanyId").OnTable("CompanyAttributeLinks");
            Delete.Table("CompanyAttributeLinks");
        }
    }
}