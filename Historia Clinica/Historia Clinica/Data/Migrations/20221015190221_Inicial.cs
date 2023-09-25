using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Historia_Clinica.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Altura = table.Column<int>(type: "int", nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EpicrisisId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Recomendacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Epicrises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    fechaYHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epicrises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Episodios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Motivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaYHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechayHoraAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechayHoraCierre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoAbierto = table.Column<bool>(type: "bit", nullable: false),
                    EvolucionId = table.Column<int>(type: "int", nullable: false),
                    EpicrisisId = table.Column<int>(type: "int", nullable: false),
                    EmpleadoRegistraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodios_Epicrises_EpicrisisId",
                        column: x => x.EpicrisisId,
                        principalTable: "Epicrises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionId = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legajo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: true),
                    ObraSocial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EpisodioId = table.Column<int>(type: "int", nullable: true),
                    TipoObraSocialipoId = table.Column<int>(type: "int", nullable: true),
                    TipoObraSocial = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Direcciones_DireccionId",
                        column: x => x.DireccionId,
                        principalTable: "Direcciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persona_Episodios_EpisodioId",
                        column: x => x.EpisodioId,
                        principalTable: "Episodios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evoluciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotaId = table.Column<int>(type: "int", nullable: false),
                    DescripcionAtencion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaYHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechayHoraAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechayHoraCierre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoAbierto = table.Column<bool>(type: "bit", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    EpisodioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evoluciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evoluciones_Episodios_EpisodioId",
                        column: x => x.EpisodioId,
                        principalTable: "Episodios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Evoluciones_Persona_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mensaje = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaYHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvolucionId = table.Column<int>(type: "int", nullable: true),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notas_Evoluciones_EvolucionId",
                        column: x => x.EvolucionId,
                        principalTable: "Evoluciones",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notas_Persona_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_EpicrisisId",
                table: "Diagnosticos",
                column: "EpicrisisId");

            migrationBuilder.CreateIndex(
                name: "IX_Epicrises_MedicoId",
                table: "Epicrises",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodios_EmpleadoRegistraId",
                table: "Episodios",
                column: "EmpleadoRegistraId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodios_EpicrisisId",
                table: "Episodios",
                column: "EpicrisisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evoluciones_EpisodioId",
                table: "Evoluciones",
                column: "EpisodioId");

            migrationBuilder.CreateIndex(
                name: "IX_Evoluciones_MedicoId",
                table: "Evoluciones",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_EmpleadoId",
                table: "Notas",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_EvolucionId",
                table: "Notas",
                column: "EvolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_DireccionId",
                table: "Persona",
                column: "DireccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_EpisodioId",
                table: "Persona",
                column: "EpisodioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnosticos_Epicrises_EpicrisisId",
                table: "Diagnosticos",
                column: "EpicrisisId",
                principalTable: "Epicrises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Epicrises_Persona_MedicoId",
                table: "Epicrises",
                column: "MedicoId",
                principalTable: "Persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Episodios_Persona_EmpleadoRegistraId",
                table: "Episodios",
                column: "EmpleadoRegistraId",
                principalTable: "Persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodios_Epicrises_EpicrisisId",
                table: "Episodios");

            migrationBuilder.DropForeignKey(
                name: "FK_Episodios_Persona_EmpleadoRegistraId",
                table: "Episodios");

            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Evoluciones");

            migrationBuilder.DropTable(
                name: "Epicrises");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "Episodios");
        }
    }
}
