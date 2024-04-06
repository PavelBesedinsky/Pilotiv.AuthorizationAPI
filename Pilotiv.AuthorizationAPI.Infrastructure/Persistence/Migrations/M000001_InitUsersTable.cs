using FluentMigrator;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(0)]
public class M000001_InitUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Email").AsString(255).NotNullable();
    }
}