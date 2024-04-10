using System.Data;
using FluentMigrator;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(0, "Users table creation")]
public class M000000_InitUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table("Users")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Login").AsString(255).NotNullable()
            .WithColumn("PasswordHash").AsString(255).Nullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable()
            .WithColumn("AuthorizationDate").AsDateTime().NotNullable()
            .WithColumn("VkUserId").AsGuid().Nullable();
    }
}