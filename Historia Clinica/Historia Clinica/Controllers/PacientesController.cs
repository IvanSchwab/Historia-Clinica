using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Historia_Clinica.ViewModels;
using Microsoft.AspNetCore.Identity;
using Historia_Clinica.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;


namespace Historia_Clinica.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {
        #region Properties & Constructor
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly HistoriaClinicaContext _context;

        public PacientesController(
           UserManager<Persona> usermanager,
            SignInManager<Persona> signInManager,
            HistoriaClinicaContext context)
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;
            this._context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string pacienteBuscado)
        {
            if (User.IsInRole("Paciente"))
            {
                return RedirectToAction("Details");
            }

            if (string.IsNullOrEmpty(pacienteBuscado))
            {
                var historiaClinicaContext = _context.Pacientes.Include(p => p.Direccion).Include(p => p.Episodios).ToList();
                return View(historiaClinicaContext);
            }
            else
            {
                var historiaClinicaContext = _context.Pacientes.Include(p => p.Direccion).Include(p => p.Episodios).Where
                    (e => e.Apellido.Contains(pacienteBuscado)).ToList();
                return View(historiaClinicaContext);
            }
            
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (User.IsInRole("Paciente"))
            {
                id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = _context.Pacientes
                .Include(p => p.Direccion)
                .Include(p => p.Episodios)
                .FirstOrDefault(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult Create()
        {
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id");
            ViewData["EpisodioId"] = new SelectList(_context.Episodios, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public async Task<IActionResult> Create([Bind("TipoObraSocial,Nombre,Apellido,Password,DNI,Telefono,Email,Calle,Altura,Localidad")]
        RegistroPacienteSistema viewModel)
        {
            if (ModelState.IsValid)
            {
                Direccion direccion = new Direccion()
                {
                    Calle = viewModel.Calle,
                    Altura = viewModel.Altura,
                    Localidad = viewModel.Localidad
                };

                _context.Direcciones.Add(direccion);
                _context.SaveChanges();

                Paciente pacienteACrear = new Paciente()
                {
                    TipoObraSocial = viewModel.TipoObraSocial,
                    Nombre = viewModel.Nombre,
                    Apellido= viewModel.Apellido,
                    DNI = viewModel.DNI,
                    Telefono = viewModel.Telefono,
                    Email = viewModel.Email,
                    UserName = viewModel.Email, 
                    DireccionId = direccion.Id
                };

                var resultadoCreate = await _usermanager.CreateAsync(pacienteACrear, Config.PasswordGenerica); 
                //Devuelve un IdentityResult. Tmb sobrecargamos con el pass que recibe para que haga el hasheo y lo guarde el mismo create. 

                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _usermanager.AddToRoleAsync(pacienteACrear, Config.PacienteRolname);
                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"Error al cargar Rol de {Config.PacienteRolname}");
                    }
                    return RedirectToAction(nameof(Index)); //Redireccionamos.
                }

                //Si la registracion no fue exitosa, en el IdentityResult tengo errores. 
                foreach (var error in resultadoCreate.Errors)
                {
                    //Proceso los errores al momento de crear.
                    ModelState.AddModelError(String.Empty, error.Description); //No lo asocio a una propiedad particular, y dependiendo de cual, mando el mensaje. 
                }

            }
            return View(viewModel);
        }
        #endregion

        #region Edit
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.PacienteRolname}, {Config.AdminRolName}")]
        public IActionResult Edit(int? id)
        {
            if (User.IsInRole("Paciente")){
                var userActual = Int32.Parse(_usermanager.GetUserId(User));
                var pacienteEdit = _context.Pacientes.Find(id);
                if (userActual != pacienteEdit.Id)
                {
                    return RedirectToAction("AccesoDenegado", "Account");
                }
            }
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", paciente.DireccionId);

            EdicionPaciente pacienteEditar = new EdicionPaciente();

            var direccion = _context.Direcciones.Find(paciente.DireccionId);
            pacienteEditar.Calle = direccion.Calle;
            pacienteEditar.Altura = direccion.Altura;
            pacienteEditar.Localidad = direccion.Localidad;
            pacienteEditar.Nombre = paciente.Nombre;
            pacienteEditar.Apellido = paciente.Apellido;
            pacienteEditar.DNI = paciente.DNI;
            pacienteEditar.TipoObraSocial = paciente.TipoObraSocial;
            pacienteEditar.Telefono = paciente.Telefono;
            pacienteEditar.Email = paciente.Email;
            pacienteEditar.UserName = paciente.UserName;
            pacienteEditar.DireccionId = direccion.Id;

            return View(pacienteEditar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.PacienteRolname}, {Config.AdminRolName}")]
        public IActionResult Edit(int id, [Bind("Id,DireccionId,Calle,Altura,Localidad,Nombre,Apellido,DNI,TipoObraSocial,Telefono,Email,UserName")] 
        EdicionPaciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }
                if (ModelState.IsValid)
            {
                try
                {
                    var pacienteEnDB = _context.Pacientes.Find(paciente.Id);
                    if (pacienteEnDB != null)
                    {

                        Direccion direccion = new Direccion();
                        var direccionEnDB = _context.Direcciones.Find(pacienteEnDB.DireccionId);
                        if (direccionEnDB != null)
                        {
                            direccionEnDB.Calle = paciente.Calle;
                            direccionEnDB.Altura = paciente.Altura;
                            direccionEnDB.Localidad = paciente.Localidad;

                            _context.Direcciones.Update(direccionEnDB);
                            _context.SaveChanges();
                        }
                        pacienteEnDB.Nombre = paciente.Nombre;
                        pacienteEnDB.Apellido = paciente.Apellido;
                        pacienteEnDB.DNI = paciente.DNI;
                        pacienteEnDB.TipoObraSocial = paciente.TipoObraSocial;
                        pacienteEnDB.Telefono = paciente.Telefono;
                        pacienteEnDB.Email = paciente.Email;
                        pacienteEnDB.UserName = paciente.Email;


                        _context.Pacientes.Update(pacienteEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", paciente.DireccionId);
            return View(paciente);
        }
        #endregion

        #region Delete
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }
            var paciente = _context.Pacientes
                .Include(p => p.Direccion)
                .Include(p => p.Episodios)
                .FirstOrDefault(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var paciente = _context.Pacientes.Find(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool PacienteExists(int id)
        {
          return _context.Pacientes.Any(e => e.Id == id);
        }
        #endregion
    }
}
