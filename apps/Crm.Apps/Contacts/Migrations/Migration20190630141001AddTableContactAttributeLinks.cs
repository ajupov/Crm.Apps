using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630141001)]
    public class Migration20190630141001AddTableContactAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("ContactAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactAttributeLinks_Id")
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_ContactAttributeLinks_ContactId")
                .FromTable("ContactAttributeLinks").ForeignColumn("ContactId")
                .ToTable("Contacts").PrimaryColumn("Id");

            Create.ForeignKey("FK_ContactAttributeLinks_AttributeId")
                .FromTable("ContactAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("ContactAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_ContactAttributeLinks_ContactId_AttributeId").OnTable("ContactAttributeLinks")
                .Columns("ContactId", "AttributeId");

            Create.Index("IX_ContactAttributeLinks_ContactId_AttributeId_CreateDateTime")
                .OnTable("ContactAttributeLinks")
                .OnColumn("ContactId").Descending()
                .OnColumn("AttributeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactAttributeLinks_ContactId_AttributeId_CreateDateTime")
                .OnTable("ContactAttributeLinks");
            Delete.UniqueConstraint("UQ_ContactAttributeLinks_ContactId_AttributeId")
                .FromTable("ContactAttributeLinks");
            Delete.ForeignKey("FK_ContactAttributeLinks_AttributeId").OnTable("ContactAttributeLinks");
            Delete.ForeignKey("FK_ContactAttributeLinks_ContactId").OnTable("ContactAttributeLinks");
            Delete.Table("ContactAttributeLinks");
        }
    }
}