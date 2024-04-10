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
            .ForeignKey("VkUserId")
            .FromTable("Users")
            .ForeignColumn("VkUserId")
            .ToTable("VkUsers")
            .PrimaryColumn("Id")
            .OnDelete(Rule.SetNull);
    }
}