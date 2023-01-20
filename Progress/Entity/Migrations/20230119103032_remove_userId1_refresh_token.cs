using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class removeuserId1refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_user_id1",
                schema: "auth",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "IX_refresh_tokens_user_id1",
                schema: "auth",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "user_id1",
                schema: "auth",
                table: "refresh_tokens");


            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                schema: "auth",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_user_id",
                schema: "auth",
                table: "refresh_tokens",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "user_id1",
                schema: "auth",
                table: "refresh_tokens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
            
            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id1",
                schema: "auth",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_user_id1",
                schema: "auth",
                table: "refresh_tokens",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
