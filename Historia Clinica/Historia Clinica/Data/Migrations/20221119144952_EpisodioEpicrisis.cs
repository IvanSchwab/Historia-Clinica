using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Historia_Clinica.Data.Migrations
{
    public partial class EpisodioEpicrisis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Evoluciones_EvolucionId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "ObraSocial",
                table: "Personas");

            migrationBuilder.AlterColumn<int>(
                name: "EvolucionId",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Evoluciones_EvolucionId",
                table: "Notas",
                column: "EvolucionId",
                principalTable: "Evoluciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Evoluciones_EvolucionId",
                table: "Notas");

            migrationBuilder.AddColumn<string>(
                name: "ObraSocial",
                table: "Personas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EvolucionId",
                table: "Notas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Evoluciones_EvolucionId",
                table: "Notas",
                column: "EvolucionId",
                principalTable: "Evoluciones",
                principalColumn: "Id");
        }
    }
}
