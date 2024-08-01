using System.Data;
using FluentMigrator;

namespace Pilotiv.AuthorizationAPI.Infrastructure.Persistence.Migrations;

[Migration(4, "Initializing refresh_tokens table.")]
public class M000004_InitRefreshTokensTable : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table("refresh_tokens")
            .WithColumn("id").AsString(88).NotNullable().PrimaryKey()
            .WithColumn("user_id").AsGuid().Nullable()
            .WithColumn("expiration_date").AsDateTime().Nullable()
            .WithColumn("created_date").AsDateTime().Nullable()
            .WithColumn("revoked_date").AsDateTime().Nullable()
            .WithColumn("created_by_ip").AsString(16).Nullable()
            .WithColumn("revoked_by_ip").AsString(16).Nullable()
            .WithColumn("revoke_reason").AsString(255).Nullable()
            .WithColumn("replacing_token_id").AsString(255).Nullable();

        Create
            .ForeignKey("user_id")
            .FromTable("refresh_tokens")
            .ForeignColumn("user_id")
            .ToTable("users")
            .PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
        
        Create
            .ForeignKey("replacing_token_id")
            .FromTable("refresh_tokens")
            .ForeignColumn("replacing_token_id")
            .ToTable("refresh_tokens")
            .PrimaryColumn("id")
            .OnDelete(Rule.SetNull);
    }
}