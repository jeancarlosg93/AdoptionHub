using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdoptionHub.Migrations
{
    /// <inheritdoc />
    public partial class editvetappointmentstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VetAppointments_FosterId",
                table: "vetappointments");

            migrationBuilder.DropForeignKey(
                name: "FK_VetAppointments_PetId",
                table: "vetappointments");

            migrationBuilder.DropForeignKey(
                name: "FK_VetAppointments_VetId",
                table: "vetappointments");

            migrationBuilder.DropColumn(
                name: "isFostered",
                table: "vetappointments");

            migrationBuilder.RenameColumn(
                name: "fosterId",
                table: "vetappointments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "FK_VetAppointments_FosterId",
                table: "vetappointments",
                newName: "IX_vetappointments_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "vetId",
                table: "vetappointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "petId",
                table: "vetappointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "apptDate",
                table: "vetappointments",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VetAppointments_PetId",
                table: "vetappointments",
                column: "petId",
                principalTable: "pets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VetAppointments_VetId",
                table: "vetappointments",
                column: "vetId",
                principalTable: "veterinarian",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vetappointments_users_UserId",
                table: "vetappointments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VetAppointments_PetId",
                table: "vetappointments");

            migrationBuilder.DropForeignKey(
                name: "FK_VetAppointments_VetId",
                table: "vetappointments");

            migrationBuilder.DropForeignKey(
                name: "FK_vetappointments_users_UserId",
                table: "vetappointments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "vetappointments",
                newName: "fosterId");

            migrationBuilder.RenameIndex(
                name: "IX_vetappointments_UserId",
                table: "vetappointments",
                newName: "FK_VetAppointments_FosterId");

            migrationBuilder.AlterColumn<int>(
                name: "vetId",
                table: "vetappointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "petId",
                table: "vetappointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "apptDate",
                table: "vetappointments",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<bool>(
                name: "isFostered",
                table: "vetappointments",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VetAppointments_FosterId",
                table: "vetappointments",
                column: "fosterId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_VetAppointments_PetId",
                table: "vetappointments",
                column: "petId",
                principalTable: "pets",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_VetAppointments_VetId",
                table: "vetappointments",
                column: "vetId",
                principalTable: "veterinarian",
                principalColumn: "id");
        }
    }
}
