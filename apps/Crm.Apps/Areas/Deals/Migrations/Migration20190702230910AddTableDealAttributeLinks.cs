using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702230910)]
    public class Migration20190702230910AddTableDealAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_DealAttributeLinks_Id").OnTable("DealAttributeLinks")
                .Column("Id");

            Create.ForeignKey("FK_DealAttributeLinks_DealId")
                .FromTable("DealAttributeLinks").ForeignColumn("DealId")
                .ToTable("Deals").PrimaryColumn("Id");

            Create.ForeignKey("FK_DealAttributeLinks_AttributeId")
                .FromTable("DealAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("DealAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_DealAttributeLinks_DealId_AttributeId").OnTable("DealAttributeLinks")
                .Columns("DealId", "AttributeId");

            Create.Index("IX_DealAttributeLinks_DealId_AttributeId").OnTable("DealAttributeLinks")
                .OnColumn("DealId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributeLinks_DealId_AttributeId").OnTable("DealAttributeLinks");
            Delete.UniqueConstraint("UQ_DealAttributeLinks_DealId_AttributeId").FromTable("DealAttributeLinks");
            Delete.ForeignKey("FK_DealAttributeLinks_AttributeId").OnTable("DealAttributeLinks");
            Delete.ForeignKey("FK_DealAttributeLinks_DealId").OnTable("DealAttributeLinks");
            Delete.PrimaryKey("PK_DealAttributeLinks_Id").FromTable("DealAttributeLinks");
            Delete.Table("DealAttributeLinks");
        }
    }
}