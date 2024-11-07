using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class removevetAppfromuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vetappointments_users_UserId",
                table: "vetappointments");

            migrationBuilder.DropIndex(
                name: "IX_vetappointments_UserId",
                table: "vetappointments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "vetappointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "vetappointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_vetappointments_UserId",
                table: "vetappointments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_vetappointments_users_UserId",
                table: "vetappointments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
