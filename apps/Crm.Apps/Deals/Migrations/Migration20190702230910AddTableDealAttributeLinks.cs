using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702230910)]
    public class Migration20190702230910AddTableDealAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealAttributeLinks_Id")
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_DealAttributeLinks_DealId")
                .FromTable("DealAttributeLinks").ForeignColumn("DealId")
                .ToTable("Deals").PrimaryColumn("Id");

            Create.ForeignKey("FK_DealAttributeLinks_AttributeId")
                .FromTable("DealAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("DealAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_DealAttributeLinks_DealId_AttributeId").OnTable("DealAttributeLinks")
                .Columns("DealId", "AttributeId");

            Create.Index("IX_DealAttributeLinks_DealId_AttributeId_CreateDateTime").OnTable("DealAttributeLinks")
                .OnColumn("DealId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributeLinks_DealId_AttributeId_CreateDateTime").OnTable("DealAttributeLinks");
            Delete.UniqueConstraint("UQ_DealAttributeLinks_DealId_AttributeId").FromTable("DealAttributeLinks");
            Delete.ForeignKey("FK_DealAttributeLinks_AttributeId").OnTable("DealAttributeLinks");
            Delete.ForeignKey("FK_DealAttributeLinks_DealId").OnTable("DealAttributeLinks");
            Delete.Table("DealAttributeLinks");
        }
    }
}