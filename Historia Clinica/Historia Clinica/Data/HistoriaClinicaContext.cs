using Historia_Clinica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Historia_Clinica.Data
{
    public class HistoriaClinicaContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public HistoriaClinicaContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas"); //Resuelve la utilizacion de ASPNETUSERS
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            //esto corresponde a como se resuelve el legajo automatico//
            //#region secuencia
            //modelBuilder.HasSequence<int>("LegajoEmpleado").StartsAt(00000001).IncrementsBy(1);
            //modelBuilder.Entity<Empleado>().Property(e => e.Legajo).HasDefaultValueSql("NEXT VALUE FOR LegajoEmpleado");

            //#endregion
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Empleado> Empleados {  get ; set; }
        public DbSet<Persona> Medicos { get; set; }
        public DbSet<Direccion> Direcciones  { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<Epicrisis> Epicrises { get; set; }
        public DbSet<Episodio> Episodios { get; set; }
        public DbSet<Evolucion> Evoluciones { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Historia_Clinica.Models.Medico> Medico { get; set; }

        public DbSet<Rol> Roles { get; set; } //Warning no deseado. 

    }
}