using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class fosterparent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FosterParentId",
                table: "pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pets_FosterParentId",
                table: "pets",
                column: "FosterParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_pets_users_FosterParentId",
                table: "pets",
                column: "FosterParentId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pets_users_FosterParentId",
                table: "pets");

            migrationBuilder.DropIndex(
                name: "IX_pets_FosterParentId",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "FosterParentId",
                table: "pets");
        }
    }
}
