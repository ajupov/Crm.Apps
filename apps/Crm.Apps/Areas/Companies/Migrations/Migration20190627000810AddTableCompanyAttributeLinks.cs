using FluentMigrator;

namespace Crm.Apps.Areas.Companies.Migrations
{
    [Migration(20190627000810)]
    public class Migration20190627000810AddTableCompanyAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("CompanyAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_CompanyAttributeLinks_Id").OnTable("CompanyAttributeLinks")
                .Column("Id");

            Create.ForeignKey("FK_CompanyAttributeLinks_CompanyId")
                .FromTable("CompanyAttributeLinks").ForeignColumn("CompanyId")
                .ToTable("Companies").PrimaryColumn("Id");

            Create.ForeignKey("FK_CompanyAttributeLinks_AttributeId")
                .FromTable("CompanyAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("CompanyAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_CompanyAttributeLinks_CompanyId_AttributeId").OnTable("CompanyAttributeLinks")
                .Columns("CompanyId", "AttributeId");

            Create.Index("IX_CompanyAttributeLinks_CompanyId_AttributeId")
                .OnTable("CompanyAttributeLinks")
                .OnColumn("CompanyId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CompanyAttributeLinks_CompanyId_AttributeId").OnTable("CompanyAttributeLinks");
            Delete.UniqueConstraint("UQ_CompanyAttributeLinks_CompanyId_AttributeId")
                .FromTable("CompanyAttributeLinks");
            Delete.ForeignKey("FK_CompanyAttributeLinks_AttributeId").OnTable("CompanyAttributeLinks");
            Delete.ForeignKey("FK_CompanyAttributeLinks_CompanyId").OnTable("CompanyAttributeLinks");
            Delete.PrimaryKey("PK_CompanyAttributeLinks_Id").FromTable("CompanyAttributeLinks");
            Delete.Table("CompanyAttributeLinks");
        }
    }
}