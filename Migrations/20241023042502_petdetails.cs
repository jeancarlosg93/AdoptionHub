using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class petdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adoptionFee",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "breed",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "color",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "dateArrived",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "dateOfBirth",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "name",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "species",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "temperament",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "pets");

            migrationBuilder.CreateTable(
                name: "petdetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    species = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    breed = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    gender = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    weight = table.Column<float>(type: "float", nullable: true),
                    color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    temperament = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dateArrived = table.Column<DateOnly>(type: "date", nullable: true),
                    bio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    adoptionFee = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_petdetails_pets_id",
                        column: x => x.id,
                        principalTable: "pets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "petdetails");

            migrationBuilder.AddColumn<decimal>(
                name: "adoptionFee",
                table: "pets",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "pets",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "breed",
                table: "pets",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "pets",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "dateArrived",
                table: "pets",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateOfBirth",
                table: "pets",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "pets",
                type: "char(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "pets",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "species",
                table: "pets",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "temperament",
                table: "pets",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "weight",
                table: "pets",
                type: "float",
                nullable: true);
        }
    }
}
