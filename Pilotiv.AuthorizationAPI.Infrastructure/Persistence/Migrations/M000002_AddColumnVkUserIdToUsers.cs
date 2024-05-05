using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(2, @"Add vk_user_id column to users table.")]
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