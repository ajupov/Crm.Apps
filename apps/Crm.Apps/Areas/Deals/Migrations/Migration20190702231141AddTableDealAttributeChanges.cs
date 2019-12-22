using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231141)]
    public class Migration20190702231141AddTableDealAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealAttributeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_DealAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("DealAttributeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributeChanges_ChangerUserId_AttributeId_CreateDateTime")
                .OnTable("DealAttributeChanges");
            Delete.Table("DealAttributeChanges");
        }
    }
}