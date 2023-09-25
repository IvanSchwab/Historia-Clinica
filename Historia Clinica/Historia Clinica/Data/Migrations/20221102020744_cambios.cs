using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Historia_Clinica.Data.Migrations
{
    public partial class cambios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodios_Epicrises_EpicrisisId",
                table: "Episodios");

            migrationBuilder.DropIndex(
                name: "IX_Episodios_EpicrisisId",
                table: "Episodios");

            migrationBuilder.DropColumn(
                name: "EpicrisisId",
                table: "Episodios");

            migrationBuilder.DropColumn(
                name: "EvolucionId",
                table: "Episodios");

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Notas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "EpisodioId",
                table: "Epicrises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Epicrises_EpisodioId",
                table: "Epicrises",
                column: "EpisodioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Epicrises_Episodios_EpisodioId",
                table: "Epicrises",
                column: "EpisodioId",
                principalTable: "Episodios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Epicrises_Episodios_EpisodioId",
                table: "Epicrises");

            migrationBuilder.DropIndex(
                name: "IX_Epicrises_EpisodioId",
                table: "Epicrises");

            migrationBuilder.DropColumn(
                name: "EpisodioId",
                table: "Epicrises");

            migrationBuilder.AlterColumn<string>(
                name: "Mensaje",
                table: "Notas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EpicrisisId",
                table: "Episodios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvolucionId",
                table: "Episodios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Episodios_EpicrisisId",
                table: "Episodios",
                column: "EpicrisisId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodios_Epicrises_EpicrisisId",
                table: "Episodios",
                column: "EpicrisisId",
                principalTable: "Epicrises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
