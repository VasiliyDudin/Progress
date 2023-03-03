using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class roleschangedtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roles",
                schema: "auth",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "role",
                schema: "auth",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                schema: "auth",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "roles",
                schema: "auth",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
