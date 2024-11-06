using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class addadoptionapplicationdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "AdoptionApplications_ibfk_1",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "applicationDate",
                table: "adoptionapplications");

            migrationBuilder.DropIndex(
                  name: "adopterId",
                  table: "adoptionapplications");

            migrationBuilder.DropColumn(
                  name: "adopterId",
                  table: "adoptionapplications");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "adoptionapplications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "applicationDateTime",
                table: "adoptionapplications",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comments",
                table: "adoptionapplications",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "adoptionapplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "adoptionapplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "adoptionapplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "adoptionapplications",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the newly added columns
            migrationBuilder.DropColumn(
                name: "address",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "applicationDateTime",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "comments",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "email",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "adoptionapplications");

            // Re-add the dropped columns and set their properties
            migrationBuilder.AddColumn<int>(
                name: "adopterId",
                table: "adoptionapplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "applicationDate",
                table: "adoptionapplications",
                type: "date",
                nullable: true);

            // Re-create the index for adopterId
            migrationBuilder.CreateIndex(
                name: "adopterId",
                table: "adoptionapplications",
                column: "adopterId");

            // Re-add the foreign key constraint for adopterId
            migrationBuilder.AddForeignKey(
                name: "AdoptionApplications_ibfk_1",
                table: "adoptionapplications",
                column: "adopterId",
                principalTable: "users",
                principalColumn: "id");
        }

    }
}
