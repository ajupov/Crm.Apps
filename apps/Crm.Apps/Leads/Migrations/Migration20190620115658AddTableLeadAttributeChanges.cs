using FluentMigrator;

namespace Crm.Apps.Leads.Migrations
{
    [Migration(20190620115658)]
    public class Migration20190620115658AddTableLeadAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("LeadAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_LeadAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_LeadAttributeChanges_ChangerUserId_AttributeId_CreateDateTime").OnTable("LeadAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_LeadAttributeChanges_ChangerUserId_AttributeId_CreateDateTime").OnTable("LeadAttributeChanges");
            Delete.Table("LeadAttributeChanges");
        }
    }
}