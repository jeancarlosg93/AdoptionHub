using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class addcityprovincecountryadoptionapplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "adoptionapplications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "adoptionapplications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "province",
                table: "adoptionapplications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "country",
                table: "adoptionapplications");

            migrationBuilder.DropColumn(
                name: "province",
                table: "adoptionapplications");
        }
    }
}
