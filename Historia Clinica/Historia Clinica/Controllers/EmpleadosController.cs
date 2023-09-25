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
using System.Net.Sockets;

namespace Historia_Clinica.Controllers
{
    [Authorize(Roles = $"{Config.AdminRolName},{Config.EmpleadoRolName}")]
    public class EmpleadosController : Controller
    {

        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;

        public EmpleadosController(HistoriaClinicaContext context, UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            this._context = context;
            this._usermanager = usermanager;
            this._signInManager = signInManager;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string empleadoBuscado)
        {
            if (string.IsNullOrEmpty(empleadoBuscado))
            {
                var historiaClinicaContext = _context.Empleados.Include(e => e.Direccion).ToList();
                return View(historiaClinicaContext.ToList());
            }
            else
            {
                var historiaClinicaContext = _context.Empleados.Include(p => p.Direccion)
                    .Where(e => e.Apellido.Contains(empleadoBuscado)).ToList();
                return View(historiaClinicaContext.ToList());
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }
            else
            {
            var empleado = _context.Empleados
                .Include(e => e.Direccion)
                .FirstOrDefault(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }
            else
            {
                return View(empleado);
            }
            }
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DNI,Telefono,Email,Calle,Altura,Localidad,FechaAlta,Password,UserName")]
        RegistroEmpleado registroEmpleado)
        {
            if (ModelState.IsValid)
            {
                Direccion direccion = new Direccion()
                {
                    Calle = registroEmpleado.Calle,
                    Altura = registroEmpleado.Altura,
                    Localidad = registroEmpleado.Localidad
                };
                _context.Direcciones.Add(direccion);
                _context.SaveChanges();
                Empleado empleadoACrear = new Empleado()
                {
                    Legajo = UltimoLegajo() + 1,
                    Nombre = registroEmpleado.Nombre,
                    Apellido = registroEmpleado.Apellido,
                    DNI = registroEmpleado.DNI,
                    Telefono = registroEmpleado.Telefono,
                    Email = registroEmpleado.Email,
                    UserName = registroEmpleado.Email,
                    DireccionId = direccion.Id,
                    FechaAlta = DateTime.Now
                };
                var resultadoCreate = await _usermanager.CreateAsync(empleadoACrear, Config.PasswordGenerica);
                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _usermanager.AddToRoleAsync(empleadoACrear, Config.EmpleadoRolName);
                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"{ErrorMsg.ErrorAlCargarRol} {Config.EmpleadoRolName}");
                    }
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", registroEmpleado.DireccionId);
            return View(registroEmpleado);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }
            else
            {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", empleado.DireccionId);

            EdicionEmpleado empleadoEditar = new EdicionEmpleado();

            var direccion = _context.Direcciones.Find(empleado.DireccionId);
            empleadoEditar.Legajo = empleado.Legajo;
            empleadoEditar.Nombre = empleado.Nombre;
            empleadoEditar.Apellido = empleado.Apellido;
            empleadoEditar.DNI = empleado.DNI;
            empleadoEditar.Telefono = empleado.Telefono;
            empleadoEditar.Email = empleado.Email;
            empleadoEditar.Calle = direccion.Calle;
            empleadoEditar.Altura = direccion.Altura;
            empleadoEditar.Localidad = direccion.Localidad;
            empleadoEditar.DireccionId = direccion.Id;

            return View(empleadoEditar);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Legajo,Id,Nombre,Apellido,DNI,Telefono,Email,UserName,Calle,Altura,Localidad")]
        EdicionEmpleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDB = _context.Empleados.Find(empleado.Id);
                    if (empleadoEnDB != null)
                    {
                        Direccion direccion = new Direccion();
                        var direccionEnDB = _context.Direcciones.Find(empleadoEnDB.DireccionId);
                        if (direccionEnDB != null)
                        {
                            direccionEnDB.Calle = empleado.Calle;
                            direccionEnDB.Altura = empleado.Altura;
                            direccionEnDB.Localidad = empleado.Localidad;

                            _context.Direcciones.Update(direccionEnDB);
                            _context.SaveChanges();
                        }
                        empleadoEnDB.Legajo = empleado.Legajo;
                        empleadoEnDB.Nombre = empleado.Nombre;
                        empleadoEnDB.Apellido = empleado.Apellido;
                        empleadoEnDB.DNI = empleado.DNI;
                        empleadoEnDB.Telefono = empleado.Telefono;
                        empleadoEnDB.Email = empleado.Email;
                        empleadoEnDB.UserName = empleado.Email;

                        _context.Empleados.Update(empleadoEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            else
            {
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", empleado.DireccionId);
            return View(empleado);
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }
            else
            {
                var empleado = _context.Empleados
                    .Include(e => e.Direccion)
                    .FirstOrDefault(m => m.Id == id);
                if (empleado == null)
                {
                    return NotFound();
                }
                return View(empleado);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var empleado = _context.Empleados.Find(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }

        public int UltimoLegajo() //Consulta y trae el Legajo del ultimo empleado creado. Si la consulta devuelve null, asigna 0 por defecto, al cual se le suma 1 en el Create.
        {
            Empleado t = _context.Empleados.OrderBy(x => x.Legajo).LastOrDefault();
            return t != null ? t.Legajo : 0;
        }
        #endregion
    }
}
