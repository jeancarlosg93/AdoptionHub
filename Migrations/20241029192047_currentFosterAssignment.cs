using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class currentFosterAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentFosterAssignmentId",
                table: "pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pets_CurrentFosterAssignmentId",
                table: "pets",
                column: "CurrentFosterAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_pets_fosterassignments_CurrentFosterAssignmentId",
                table: "pets",
                column: "CurrentFosterAssignmentId",
                principalTable: "fosterassignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pets_fosterassignments_CurrentFosterAssignmentId",
                table: "pets");

            migrationBuilder.DropIndex(
                name: "IX_pets_CurrentFosterAssignmentId",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "CurrentFosterAssignmentId",
                table: "pets");
        }
    }
}
