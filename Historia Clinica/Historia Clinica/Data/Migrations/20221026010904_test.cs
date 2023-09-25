using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Historia_Clinica.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Episodios_EpisodioId",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_EpisodioId",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "EpisodioId",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "TipoObraSocialipoId",
                table: "Personas");

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Episodios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Episodios_PacienteId",
                table: "Episodios",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodios_Personas_PacienteId",
                table: "Episodios",
                column: "PacienteId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodios_Personas_PacienteId",
                table: "Episodios");

            migrationBuilder.DropIndex(
                name: "IX_Episodios_PacienteId",
                table: "Episodios");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Episodios");

            migrationBuilder.AddColumn<int>(
                name: "EpisodioId",
                table: "Personas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoObraSocialipoId",
                table: "Personas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_EpisodioId",
                table: "Personas",
                column: "EpisodioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Episodios_EpisodioId",
                table: "Personas",
                column: "EpisodioId",
                principalTable: "Episodios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
