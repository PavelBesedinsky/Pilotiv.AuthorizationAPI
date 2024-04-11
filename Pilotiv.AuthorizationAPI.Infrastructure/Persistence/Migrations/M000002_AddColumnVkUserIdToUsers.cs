using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

// TODO: Исправить название
[Migration(2, "Add column VkUserId to Users table")]
public class M000002_AddColumnVkUserIdToUsers : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .ForeignKey("vk_user_id")
            .FromTable("users")
            .ForeignColumn("vk_user_id")
            .ToTable("vk_users")
            .PrimaryColumn("id")
            .OnDelete(Rule.SetNull);
    }
}