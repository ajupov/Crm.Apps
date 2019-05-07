using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190506224315)]
    public class Migration20190506224315AddTableUserAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("UserAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserAttributeLinks_Id")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_UserAttributeLinks_UserId")
                .FromTable("UserAttributeLinks").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey("FK_UserAttributeLinks_AttributeId")
                .FromTable("UserAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("UserAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserAttributeLinks_UserId_AttributeId").OnTable("UserAttributeLinks")
                .Columns("UserId", "AttributeId");

            Create.Index("IX_UserAttributeLinks_UserId_AttributeId").OnTable("UserAttributeLinks")
                .OnColumn("UserId").Descending()
                .OnColumn("AttributeId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserAttributeLinks_UserId_AttributeId").OnTable("UserAttributeLinks");
            Delete.UniqueConstraint("UQ_UserAttributeLinks_UserId_AttributeId").FromTable("UserAttributeLinks");
            Delete.ForeignKey("FK_UserAttributeLinks_AttributeId").OnTable("UserAttributeLinks");
            Delete.ForeignKey("FK_UserAttributeLinks_UserId").OnTable("UserAttributeLinks");
            Delete.Table("UserAttributeLinks");
        }
    }
}