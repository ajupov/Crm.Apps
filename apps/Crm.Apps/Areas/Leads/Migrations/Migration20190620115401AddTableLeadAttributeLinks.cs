using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620115401)]
    public class Migration20190620115401AddTableLeadAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("LeadAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadAttributeLinks_Id")
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_LeadAttributeLinks_LeadId")
                .FromTable("LeadAttributeLinks").ForeignColumn("LeadId")
                .ToTable("Leads").PrimaryColumn("Id");

            Create.ForeignKey("FK_LeadAttributeLinks_AttributeId")
                .FromTable("LeadAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("LeadAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_LeadAttributeLinks_LeadId_AttributeId").OnTable("LeadAttributeLinks")
                .Columns("LeadId", "AttributeId");

            Create.Index("IX_LeadAttributeLinks_LeadId_AttributeId_CreateDateTime").OnTable("LeadAttributeLinks")
                .OnColumn("LeadId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadAttributeLinks_LeadId_AttributeId_CreateDateTime").OnTable("LeadAttributeLinks");
            Delete.UniqueConstraint("UQ_LeadAttributeLinks_LeadId_AttributeId").FromTable("LeadAttributeLinks");
            Delete.ForeignKey("FK_LeadAttributeLinks_AttributeId").OnTable("LeadAttributeLinks");
            Delete.ForeignKey("FK_LeadAttributeLinks_LeadId").OnTable("LeadAttributeLinks");
            Delete.Table("LeadAttributeLinks");
        }
    }
}