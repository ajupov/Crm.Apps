using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115658)]
    public class Migration20190620115658AddTableLeadAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_LeadAttributeChanges_Id").OnTable("LeadAttributeChanges")
                .Column("Id");

            Create.Index("IX_LeadAttributeChanges_AttributeId_CreateDateTime").OnTable("LeadAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadAttributeChanges_AttributeId_CreateDateTime").OnTable("LeadAttributeChanges");
            Delete.PrimaryKey("PK_LeadAttributeChanges_Id").FromTable("LeadAttributeChanges");
            Delete.Table("LeadAttributeChanges");
        }
    }
}