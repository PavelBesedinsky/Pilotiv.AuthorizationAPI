using FluentMigrator;
using FluentMigrator.Postgres;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(2, "Initializing vk_users table.")]
public class M000002_InitVkUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("vk_users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("internal_id").AsString(255).Nullable();
    }
}