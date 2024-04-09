using FluentMigrator;
using FluentMigrator.Postgres;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(1, "VkUsers table creation")]
public class M000001_InitVkUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("VkUsers")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("InternalId").AsString(255).NotNullable();
    }
}