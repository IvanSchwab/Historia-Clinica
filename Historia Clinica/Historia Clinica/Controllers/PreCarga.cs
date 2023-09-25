using Historia_Clinica.Data;
using Historia_Clinica.Helpers;
using Historia_Clinica.Models;
using Historia_Clinica.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Historia_Clinica.Controllers
{
    [AllowAnonymous]
    public class PreCarga : Controller
    {
        #region Properties & Constructor
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly HistoriaClinicaContext _context;

        private List<string> roles = new List<string>() { Config.PacienteRolname, Config.EmpleadoRolName, Config.MedicoRolName, Config.AdminRolName };

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, HistoriaClinicaContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }
        #endregion

        #region Pre-Carga
        public IActionResult Carga()
        //Crear los roles 
        {

            CrearRoles().Wait();
            CrearAdmin().Wait();
            CrearEmpleados().Wait();
            CrearPacientes().Wait();
            CrearMedicos().Wait();
            CrearEpisodios();
            CrearEvoluciones();
            CrearNotas();

            return RedirectToAction("Index", "Home", new { mensaje = "Proceso de Carga Finalizado" });
        }
        #endregion

        #region Crear Medicos
        private async Task CrearMedicos()
        {
            Direccion direccion = new Direccion()
            {
                Calle = "Libertador",
                Altura = 1100,
                Localidad = "CABA",
            };
            _context.Direcciones.Add(direccion);
            _context.SaveChanges();


            Medico medico = new Medico()
            {
                Matricula = "AA001122",
                Tipo = TipoEspecialidadMedica.CLINICA_MEDICA,
                Legajo = 5,
                Nombre = "Homero",
                Apellido = "Simpson",
                DNI = "11123123",
                Telefono = "+5491122334455",
                Email = "medico@ort.edu.ar",
                UserName = "medico@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now,
            };

            Medico medico2 = new Medico()
            {
                Matricula = "BB112233",
                Tipo = TipoEspecialidadMedica.DERMATOLOGIA,
                Legajo = 6,
                Nombre = "Montgomery",
                Apellido = "Burns",
                DNI = "12123123",
                Telefono = "+5491122334455",
                Email = "medico1@ort.edu.ar",
                UserName = "medico1@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now,
            };

            Medico medico3 = new Medico()
            {
                Matricula = "CC223344",
                Tipo = TipoEspecialidadMedica.CARDIOLOGIA,
                Legajo = 7,
                Nombre = "Apu",
                Apellido = "Nahasapeemapetilon",
                DNI = "13123123",
                Telefono = "+5491122334455",
                Email = "medico2@ort.edu.ar",
                UserName = "medico2@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now,
            };

            if (_context.Medicos.Any(p => p.DNI == "11123123"))
            {

            }
            else
            {
                await _userManager.CreateAsync(medico, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(medico, Config.MedicoRolName);

                await _userManager.CreateAsync(medico2, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(medico2, Config.MedicoRolName);

                await _userManager.CreateAsync(medico3, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(medico3, Config.MedicoRolName);
            }
        }
        #endregion

        #region Crear Pacientes
        private async Task CrearPacientes()
        {
            Direccion direccion = new Direccion()
            {
                Calle = "Libertador",
                Altura = 1100,
                Localidad = "CABA",
            };
            _context.Direcciones.Add(direccion);
            _context.SaveChanges();

            Paciente paciente = new Paciente()
            {
                Nombre = "Marge",
                Apellido = "Simpson",
                DNI = "14123123",
                Telefono = "+5491155443322",
                Email = "paciente@ort.edu.ar",
                UserName = "paciente@ort.edu.ar",
                TipoObraSocial = TipoObraSocial.SIN_OBRA_SOCIAL,
                DireccionId = direccion.Id
            };

            Paciente paciente2 = new Paciente()
            {
                Nombre = "Waylon",
                Apellido = "Smithers",
                DNI = "16123123",
                Telefono = "+5491155443322",
                Email = "paciente1@ort.edu.ar",
                UserName = "paciente1@ort.edu.ar",
                TipoObraSocial = TipoObraSocial.OBRA_SOCIAL_FERROVIARIA,
                DireccionId = direccion.Id
            };

            Paciente paciente3 = new Paciente()
            {
                Nombre = "Kent",
                Apellido = "Brockman",
                DNI = "17123123",
                Telefono = "+5491155443322",
                Email = "paciente2@ort.edu.ar",
                UserName = "paciente2@ort.edu.ar",
                TipoObraSocial = TipoObraSocial.OBRA_SOCIAL_UNION_PERSONAL_DE_LA_UNION_DEL_PERSONAL_CIVIL_DE_LA_NACION,
                DireccionId = direccion.Id
            };

            if (_context.Pacientes.Any(p => p.DNI == "14123123"))
            {
                //Evaluar que accion tomar
            }
            else
            {
                await _userManager.CreateAsync(paciente, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(paciente, Config.PacienteRolname);

                await _userManager.CreateAsync(paciente2, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(paciente2, Config.PacienteRolname);

                await _userManager.CreateAsync(paciente3, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(paciente3, Config.PacienteRolname);
            }
        }
        #endregion

        #region Crear Empleados
        private async Task CrearEmpleados()
        {
            Direccion direccion = new Direccion()
            {
                Calle = "Libertador",
                Altura = 1100,
                Localidad = "CABA",
            };
            _context.Direcciones.Add(direccion);
            _context.SaveChanges();

            Empleado empleado = new Empleado()
            {
                Legajo = 2,
                Nombre = "Ned",
                Apellido = "Flanders",
                DNI = "18123123",
                Telefono = "+5491133224455",
                Email = "empleado@ort.edu.ar",
                UserName = "empleado@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now
            };

            Empleado empleado2 = new Empleado()
            {
                Legajo = 3,
                Nombre = "Milhousen",
                Apellido = "Van Houte",
                DNI = "19123123",
                Telefono = "+5491133224455",
                Email = "empleado1@ort.edu.ar",
                UserName = "empleado1@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now
            };

            Empleado empleado3 = new Empleado()
            {
                Legajo = 4,
                Nombre = "Moe",
                Apellido = "Szyslak",
                DNI = "20123123",
                Telefono = "+5491133224455",
                Email = "empleado2@ort.edu.ar",
                UserName = "empleado2@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now
            };

            if (_context.Empleados.Any(p => p.DNI == "18123123"))
            {
                //Evaluar que accion tomar
            }
            else
            {
                await _userManager.CreateAsync(empleado, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(empleado, Config.EmpleadoRolName);

                await _userManager.CreateAsync(empleado2, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(empleado2, Config.EmpleadoRolName);

                await _userManager.CreateAsync(empleado3, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(empleado3, Config.EmpleadoRolName);
            }

        }
        #endregion

        #region Crear Admin
        private async Task CrearAdmin()
        {
            Direccion direccion = new Direccion()
            {
                Calle = "Libertador",
                Altura = 1100,
                Localidad = "CABA",
            };
            _context.Direcciones.Add(direccion);
            _context.SaveChanges();

            Empleado admin = new Empleado()
            {
                Legajo = 1,
                Nombre = "Nelson",
                Apellido = "Muntz",
                DNI = "21123123",
                Telefono = "+5491144553322",
                Email = "admin@ort.edu.ar",
                UserName = "admin@ort.edu.ar",
                DireccionId = direccion.Id,
                FechaAlta = DateTime.Now
            };

            if (_context.Empleados.Any(p => p.DNI == "21123123"))
            {
                //Evaluar que accion tomar
            }
            else
            {
                await _userManager.CreateAsync(admin, Config.PasswordGenerica);
                await _userManager.AddToRoleAsync(admin, Config.AdminRolName);
            }
        }
        #endregion

        #region Crear Episodios
        private void CrearEpisodios()
        {
            if (_context.Episodios.Any(e => e.Id == 1))
            {
                //Evaluar que accion tomar
            }
            else
            {
                Episodio episodio = new Episodio()
                {
                    Motivo = "Fuerte dolor en brazo izquierdo",
                    Descripcion = "Accidente domentisco",
                    EstadoAbierto = true,
                    EmpleadoRegistraId = 2,
                    PacienteId = 5,
                };

                _context.Episodios.Add(episodio);
                _context.SaveChanges();

                Episodio episodio2 = new Episodio()
                {
                    Motivo = "Fractura expuesta",
                    Descripcion = "Accidente en la via publica",
                    EstadoAbierto = true,
                    EmpleadoRegistraId = 3,
                    PacienteId = 6,
                };

                _context.Episodios.Add(episodio2);
                _context.SaveChanges();

                Episodio episodio3 = new Episodio()
                {
                    Motivo = "Falta de aire, fiebre",
                    Descripcion = "Sospecha COVID",
                    EstadoAbierto = true,
                    EmpleadoRegistraId = 4,
                    PacienteId = 7,
                };

                _context.Episodios.Add(episodio3);
                _context.SaveChanges();
            }
        }
        #endregion

        #region Crear Evoluciones
        private void CrearEvoluciones()
        {
            if (_context.Evoluciones.Any(e => e.Id == 1))
            {
                //Evaluar que accion tomar
            }
            else
            {
                Evolucion evolucion = new Evolucion()
                {
                    DescripcionAtencion = "Se envia paciente a Rx",
                    FechayHoraAlta = DateTime.Now.AddDays(2),
                    EstadoAbierto = true,
                    MedicoId = 8,
                    EpisodioId = 1,
                };

                _context.Evoluciones.Add(evolucion);
                _context.SaveChanges();

                Evolucion evolucion2 = new Evolucion()
                {
                    DescripcionAtencion = "Se envia paciente a Rx",
                    FechayHoraAlta = DateTime.Now.AddDays(2),
                    EstadoAbierto = true,
                    MedicoId = 9,
                    EpisodioId = 2,
                };

                _context.Evoluciones.Add(evolucion2);
                _context.SaveChanges();

                Evolucion evolucion3 = new Evolucion()
                {
                    DescripcionAtencion = "Indica placa Torax e hisopado",
                    FechayHoraAlta = DateTime.Now.AddDays(2),
                    EstadoAbierto = true,
                    MedicoId = 10,
                    EpisodioId = 3,
                };

                _context.Evoluciones.Add(evolucion3);
                _context.SaveChanges();
            }
        }
        #endregion

        #region Crear Notas
        private void CrearNotas()
        {
            if (_context.Notas.Any(e => e.Id == 1))
            {
                //Evaluar que accion tomar
            }
            else
            {
                Nota nota = new Nota()
                {
                    Mensaje = "Solicita comida sin T.A.C.C",
                    FechaYHora = DateTime.Now,
                    EvolucionId = 1,
                    EmpleadoId = 8,
                };

                _context.Notas.Add(nota);
                _context.SaveChanges();

                Nota nota2 = new Nota()
                {
                    Mensaje = "Solicita bajar temperatura A/C",
                    FechaYHora = DateTime.Now,
                    EvolucionId = 2,
                    EmpleadoId = 9,
                };

                _context.Notas.Add(nota2);
                _context.SaveChanges();

                Nota nota3 = new Nota()
                {
                    Mensaje = "Paciente vegano",
                    FechaYHora = DateTime.Now,
                    EvolucionId = 3,
                    EmpleadoId = 10,
                };

                _context.Notas.Add(nota3);
                _context.SaveChanges();

            }
        }
        #endregion

        #region Crear Roles
        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                    await _roleManager.CreateAsync(new Rol(rolName));
                }
            }
        }
        #endregion
    }
}
