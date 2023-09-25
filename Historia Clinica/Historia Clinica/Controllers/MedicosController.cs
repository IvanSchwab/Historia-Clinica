using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using System.Net;
using Historia_Clinica.ViewModels;
using Microsoft.AspNetCore.Identity;
using Historia_Clinica.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Historia_Clinica.Controllers
{
    [Authorize(Roles =$"{Config.MedicoRolName}, {Config.EmpleadoRolName}, {Config.AdminRolName}")] 
    public class MedicosController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;

        public MedicosController(HistoriaClinicaContext context, UserManager<Persona> userManager, SignInManager<Persona> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string medicoBuscado)
        {
            if (string.IsNullOrEmpty(medicoBuscado))
            {
                var historiaClinicaContext = _context.Medico.Include(e => e.Direccion).ToList();
                return View(historiaClinicaContext.ToList());
            }
            else
            {
                var historiaClinicaContext = _context.Medico.Include(p => p.Direccion).Where(e => e.Apellido.Contains(medicoBuscado)).ToList();
                return View(historiaClinicaContext.ToList());
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Medico == null) 
            {
                return NotFound();
            }

            var medico = _context.Medico 
                .Include(m => m.Direccion)
                .FirstOrDefault(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult Create()
        {
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create([Bind("Matricula,Tipo,Nombre,Apellido,DNI,Telefono,Email,UserName,Calle,Altura,Localidad")] RegistroMedico registroMedico)
        {
            if (ModelState.IsValid)
            {
                Direccion direccion = new Direccion()
                {
                    Calle = registroMedico.Calle,
                    Altura = registroMedico.Altura,
                    Localidad = registroMedico.Localidad
                };

                _context.Direcciones.Add(direccion);
                _context.SaveChanges();

                Medico MedicoCrear = new Medico()
                {
                    Matricula = registroMedico.Matricula,
                    Tipo = registroMedico.Tipo,
                    Legajo = UltimoLegajo()+1, 
                    Nombre = registroMedico.Nombre,
                    Apellido = registroMedico.Apellido,
                    DNI = registroMedico.DNI,
                    Telefono = registroMedico.Telefono,
                    Email = registroMedico.Email,
                    UserName = registroMedico.Email, 
                    DireccionId = direccion.Id,
                    FechaAlta = DateTime.Now,
            };
                var resultadoNewMedico = await _userManager.CreateAsync(MedicoCrear, Config.PasswordGenerica);

                if (resultadoNewMedico.Succeeded) //IdentityResult TRUE, pudo crearlo.
                {

                    var resultadoAddRole = await _userManager.AddToRoleAsync(MedicoCrear, Config.MedicoRolName);

                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction(nameof(Index)); //Redireccionamos.
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, $"Error al cargar Rol de {Config.MedicoRolName}");
                    }
                
                }   

                //Procesamos los errores de la creacion.
                foreach (var error in resultadoNewMedico.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                    
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", registroMedico.DireccionId);
            return View(registroMedico);
        }
        #endregion

        #region
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Medico == null)
            {
                return NotFound();
            }

            var medico = _context.Medico.Find(id);
            if (medico == null)
            {
                return NotFound();
            }
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", medico.DireccionId);

            EdicionMedico medicoEdit = new EdicionMedico();
            var direccion = _context.Direcciones.Find(medico.DireccionId);
            medicoEdit.Legajo = medico.Legajo;
            medicoEdit.Nombre = medico.Nombre;
            medicoEdit.Apellido = medico.Apellido; 
            medicoEdit.Matricula = medico.Matricula;
            medicoEdit.Tipo = medico.Tipo;
            medicoEdit.DNI = medico.DNI;
            medicoEdit.Telefono = medico.Telefono;
            medicoEdit.Email = medico.Email;
            medicoEdit.Calle = direccion.Calle;
            medicoEdit.Altura = direccion.Altura;
            medicoEdit.Localidad = direccion.Localidad;
            medicoEdit.DireccionId = direccion.Id;

            return View(medicoEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Matricula,Tipo,Legajo,Id,Nombre,Apellido,DNI,Telefono,Email,UserName,Calle,Altura,Localidad")] EdicionMedico medico)
        {
            if (id != medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var medicoEnDB = _context.Medico.Find(medico.Id);
                    if(medicoEnDB != null)
                    {

                        Direccion direccion = new Direccion();
                        var direccionEnDB = _context.Direcciones.Find(medicoEnDB.DireccionId);
                        if (direccionEnDB != null)
                        {
                            direccionEnDB.Calle = medico.Calle;
                            direccionEnDB.Altura = medico.Altura;
                            direccionEnDB.Localidad = medico.Localidad;

                            _context.Direcciones.Update(direccionEnDB);
                            _context.SaveChanges();
                        }
                        
                        medicoEnDB.Matricula = medico.Matricula;
                        medicoEnDB.Tipo = medico.Tipo;
                        medicoEnDB.Legajo = medico.Legajo;
                        medicoEnDB.Nombre = medico.Nombre;
                        medicoEnDB.Apellido = medico.Apellido;
                        medicoEnDB.DNI = medico.DNI;
                        medicoEnDB.Telefono = medico.Telefono;
                        medicoEnDB.Email = medico.Email;
                        medicoEnDB.UserName = medico.Email;

                        _context.Medico.Update(medicoEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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
            ViewData["DireccionId"] = new SelectList(_context.Direcciones, "Id", "Id", medico.DireccionId);
            return View(medico);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Medico == null)
            {
                return NotFound();
            }

            var medico = _context.Medico
                .Include(m => m.Direccion)
                .FirstOrDefault(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Medico == null)
            {
                return Problem("Entity set 'HistoriaClinicaContext.Medico'  is null.");
            }
            var medico = _context.Medico.Find(id);
            if (medico != null)
            {
                _context.Medico.Remove(medico);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool MedicoExists(int id)
        {
          return _context.Medico.Any(e => e.Id == id);
        }

        public int UltimoLegajo() //Consulta y trae el Legajo del ultimo empleado creado. Si la consulta devuelve null, asigna 0 por defecto, al cual se le suma 1 en el Create.
        {
            Empleado t = _context.Empleados.OrderBy(x => x.Legajo).LastOrDefault();
            return t != null ? t.Legajo : 0;
        }
        #endregion
    }
}
