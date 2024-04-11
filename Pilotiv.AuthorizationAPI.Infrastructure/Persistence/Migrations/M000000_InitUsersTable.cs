using System.Data;
using FluentMigrator;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(0, "Users table creation")]
public class M000000_InitUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("login").AsString(255).Nullable()
            .WithColumn("password_hash").AsString(255).Nullable()
            .WithColumn("email").AsString(255).Nullable()
            .WithColumn("registration_date").AsDateTime().Nullable()
            .WithColumn("authorization_date").AsDateTime().Nullable()
            .WithColumn("vk_user_id").AsGuid().Nullable();
    }
}