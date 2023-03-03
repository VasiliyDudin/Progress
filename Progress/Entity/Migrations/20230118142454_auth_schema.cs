using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class authschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattelFields_Users_UserId",
                schema: "data",
                table: "BattelFields");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStatistics_Users_UserId",
                schema: "data",
                table: "GameStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "data",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "data",
                newName: "users",
                newSchema: "auth");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "auth",
                table: "users",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "auth",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "auth",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "auth",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IpAdress",
                schema: "auth",
                table: "users",
                newName: "ip_adress");

            migrationBuilder.AddColumn<byte[]>(
                name: "password_hash",
                schema: "auth",
                table: "users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "password_salt",
                schema: "auth",
                table: "users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "roles",
                schema: "auth",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "auth",
                table: "users",
                column: "id");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false),
                    expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    userid1 = table.Column<long>(name: "user_id1", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user_id1",
                        column: x => x.userid1,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "auth",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id1",
                schema: "auth",
                table: "refresh_tokens",
                column: "user_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_BattelFields_users_UserId",
                schema: "data",
                table: "BattelFields",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStatistics_users_UserId",
                schema: "data",
                table: "GameStatistics",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattelFields_users_UserId",
                schema: "data",
                table: "BattelFields");

            migrationBuilder.DropForeignKey(
                name: "FK_GameStatistics_users_UserId",
                schema: "data",
                table: "GameStatistics");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "auth");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "auth",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_email",
                schema: "auth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_hash",
                schema: "auth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_salt",
                schema: "auth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "roles",
                schema: "auth",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "auth",
                newName: "Users",
                newSchema: "data");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "data",
                table: "Users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "data",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "data",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "data",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ip_adress",
                schema: "data",
                table: "Users",
                newName: "IpAdress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "data",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattelFields_Users_UserId",
                schema: "data",
                table: "BattelFields",
                column: "UserId",
                principalSchema: "data",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameStatistics_Users_UserId",
                schema: "data",
                table: "GameStatistics",
                column: "UserId",
                principalSchema: "data",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
