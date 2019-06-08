﻿using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190507225048)]
    public class Migration20190507225048AddTableUserGroups : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroups")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserGroups_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_UserGroups_AccountId_Name").OnTable("UserGroups")
                .Columns("AccountId", "Name");

            Create.Index("IX_UserGroups_AccountId_Name_IsDeleted_CreateDateTime").OnTable("UserGroups")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroups_AccountId_Name_IsDeleted_CreateDateTime").OnTable("UserGroups");
            Delete.UniqueConstraint("UQ_UserGroups_AccountId_Name").FromTable("UserGroups");
            Delete.Table("UserGroups");
        }
    }
}