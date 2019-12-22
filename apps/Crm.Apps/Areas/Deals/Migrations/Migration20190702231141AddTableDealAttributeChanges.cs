using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231141)]
    public class Migration20190702231141AddTableDealAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_DealAttributeChanges_Id").OnTable("DealAttributeChanges")
                .Column("Id");

            Create.Index("IX_DealAttributeChanges_AttributeId_CreateDateTime").OnTable("DealAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributeChanges_AttributeId_CreateDateTime").OnTable("DealAttributeChanges");
            Delete.PrimaryKey("PK_DealAttributeChanges_Id").FromTable("DealAttributeChanges");
            Delete.Table("DealAttributeChanges");
        }
    }
}