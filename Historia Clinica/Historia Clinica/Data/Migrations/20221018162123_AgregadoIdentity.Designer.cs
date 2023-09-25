﻿// <auto-generated />
using System;
using Historia_Clinica.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Historia_Clinica.Data.Migrations
{
    [DbContext(typeof(HistoriaClinicaContext))]
    [Migration("20221018162123_AgregadoIdentity")]
    partial class AgregadoIdentity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Historia_Clinica.Models.Diagnostico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EpicrisisId")
                        .HasColumnType("int");

                    b.Property<string>("Recomendacion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("EpicrisisId");

                    b.ToTable("Diagnosticos");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Direccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Altura")
                        .HasColumnType("int");

                    b.Property<string>("Calle")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Localidad")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Direcciones");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Epicrisis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("MedicoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("fechaYHora")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MedicoId");

                    b.ToTable("Epicrises");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Episodio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EmpleadoRegistraId")
                        .HasColumnType("int");

                    b.Property<int>("EpicrisisId")
                        .HasColumnType("int");

                    b.Property<bool>("EstadoAbierto")
                        .HasColumnType("bit");

                    b.Property<int>("EvolucionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaYHoraInicio")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechayHoraAlta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechayHoraCierre")
                        .HasColumnType("datetime2");

                    b.Property<string>("Motivo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("EmpleadoRegistraId");

                    b.HasIndex("EpicrisisId")
                        .IsUnique();

                    b.ToTable("Episodios");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Evolucion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DescripcionAtencion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("EpisodioId")
                        .HasColumnType("int");

                    b.Property<bool>("EstadoAbierto")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaYHoraInicio")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechayHoraAlta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechayHoraCierre")
                        .HasColumnType("datetime2");

                    b.Property<int>("MedicoId")
                        .HasColumnType("int");

                    b.Property<int>("NotaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EpisodioId");

                    b.HasIndex("MedicoId");

                    b.ToTable("Evoluciones");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Nota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EmpleadoId")
                        .HasColumnType("int");

                    b.Property<int?>("EvolucionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaYHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mensaje")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("EmpleadoId");

                    b.HasIndex("EvolucionId");

                    b.ToTable("Notas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Personas", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("PersonasRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Historia_Clinica.Models.Persona", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser<int>");

                    b.Property<string>("Apellido")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DireccionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("DireccionId");

                    b.HasDiscriminator().HasValue("Persona");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Rol", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole<int>");

                    b.HasDiscriminator().HasValue("Rol");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Empleado", b =>
                {
                    b.HasBaseType("Historia_Clinica.Models.Persona");

                    b.Property<string>("Legajo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Empleado");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Paciente", b =>
                {
                    b.HasBaseType("Historia_Clinica.Models.Persona");

                    b.Property<int>("EpisodioId")
                        .HasColumnType("int");

                    b.Property<string>("ObraSocial")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TipoObraSocial")
                        .HasColumnType("int");

                    b.Property<int>("TipoObraSocialipoId")
                        .HasColumnType("int");

                    b.HasIndex("EpisodioId");

                    b.HasDiscriminator().HasValue("Paciente");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Medico", b =>
                {
                    b.HasBaseType("Historia_Clinica.Models.Empleado");

                    b.Property<string>("Matricula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Medico");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Diagnostico", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Epicrisis", "Epicrisis")
                        .WithMany()
                        .HasForeignKey("EpicrisisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Epicrisis");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Epicrisis", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medico");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Episodio", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Empleado", "EmpleadoRegistra")
                        .WithMany()
                        .HasForeignKey("EmpleadoRegistraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Historia_Clinica.Models.Epicrisis", "Epicrisis")
                        .WithOne("Episodio")
                        .HasForeignKey("Historia_Clinica.Models.Episodio", "EpicrisisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmpleadoRegistra");

                    b.Navigation("Epicrisis");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Evolucion", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Episodio", null)
                        .WithMany("Evoluciones")
                        .HasForeignKey("EpisodioId");

                    b.HasOne("Historia_Clinica.Models.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medico");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Nota", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Empleado", "Empleado")
                        .WithMany()
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Historia_Clinica.Models.Evolucion", "Evolucion")
                        .WithMany("Notas")
                        .HasForeignKey("EvolucionId");

                    b.Navigation("Empleado");

                    b.Navigation("Evolucion");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser<int>", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Historia_Clinica.Models.Persona", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Direccion", "Direccion")
                        .WithMany()
                        .HasForeignKey("DireccionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direccion");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Paciente", b =>
                {
                    b.HasOne("Historia_Clinica.Models.Episodio", "Episodio")
                        .WithMany()
                        .HasForeignKey("EpisodioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Episodio");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Epicrisis", b =>
                {
                    b.Navigation("Episodio");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Episodio", b =>
                {
                    b.Navigation("Evoluciones");
                });

            modelBuilder.Entity("Historia_Clinica.Models.Evolucion", b =>
                {
                    b.Navigation("Notas");
                });
#pragma warning restore 612, 618
        }
    }
}
