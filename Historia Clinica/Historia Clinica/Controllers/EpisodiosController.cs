using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.VisualBasic;
using System.Diagnostics.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Historia_Clinica.Helpers;

namespace Historia_Clinica.Controllers
{
    [Authorize]
    public class EpisodiosController : Controller
    {
        #region Properties & Contructor
        private readonly HistoriaClinicaContext _context;
        private readonly UserManager<Persona> _usermanager;

        public EpisodiosController(HistoriaClinicaContext context, UserManager<Persona> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(int? pacienteid)
        {
            List<Episodio> episodios;
            if (User.IsInRole("Paciente"))
            {
                var userActual = Int32.Parse(_usermanager.GetUserId(User));
                var episodioBuscado = _context.Episodios.Include(e => e.EmpleadoRegistra).
                    Include(e => e.Epicrisis).Include(e => e.Paciente).Include(e => e.Evoluciones).
                    Where(e => e.PacienteId.Equals(userActual)).ToList();
                return View(episodioBuscado);
            }
            else
            {
                episodios = new List<Episodio>();
            }
            if (pacienteid != null)
            {
                episodios = _context.Episodios.Include(e => e.EmpleadoRegistra).Include(e => e.Epicrisis).
                    Include(e => e.Paciente).Include(e => e.Evoluciones).
                    Where(e => e.PacienteId.Equals(pacienteid)).ToList();
                return View(episodios);
            }else
            {
                episodios = _context.Episodios.Include(e => e.EmpleadoRegistra).
                    Include(e => e.Epicrisis).Include(e => e.Paciente).
                    Include(e => e.Evoluciones).ToList();
                return View(episodios);
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public  IActionResult Details(int? id)
        {
            if (id == null || _context.Episodios == null)
            {
                return NotFound();
            }
            else
            {
                var episodio = _context.Episodios
               .Include(e => e.EmpleadoRegistra)
               .Include(e => e.Epicrisis)
               .Include(e => e.Paciente)
               .FirstOrDefault(m => m.Id == id);

                if (episodio == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(episodio);
                }
            } 
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult Create(int? pacienteId)
        {
            //verifico si me pasaron un pacientid
            //voy a buscar el paciente a la db 
            // si es necesario

            Episodio episodio = new Episodio() { PacienteId = pacienteId.Value };

            var empleadoActualUserName = User.Identity.Name;
            var empleadoactual = _context.Empleados.FirstOrDefault(e => e.NormalizedUserName == empleadoActualUserName.ToUpper());
            episodio.EmpleadoRegistraId = empleadoactual.Id;

            return View(episodio);
        }

        
        [HttpPost]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int? pacienteId, [Bind("Id,Motivo,Descripcion,EmpleadoRegistraId,PacienteId")] Episodio episodio)
        {
            if (ModelState.IsValid)
            {
                episodio.EstadoAbierto = true;
                _context.Episodios.Add(episodio);
                _context.SaveChanges();
                var pacienteIDfilter = episodio.PacienteId;
                return RedirectToAction("Index", "Episodios", new { pacienteid = episodio.PacienteId });
            }
            ViewData["EmpleadoRegistraId"] = new SelectList(_context.Empleados, "Id", "Fullname", episodio.EmpleadoRegistraId);


            return View(episodio);
        }
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}, {Config.MedicoRolName}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Episodios == null)
            {
                return NotFound();
            }
            var episodio =  _context.Episodios.Find(id);
            if (episodio == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoRegistraId"] = new SelectList(_context.Empleados, "Id", "Fullname", episodio.EmpleadoRegistraId);
         
            return View(episodio);
        }
       
        [HttpPost]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}, {Config.MedicoRolName}")]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("Id,Motivo,Descripcion,FechaYHoraInicio,FechayHoraAlta,FechayHoraCierre," +
            "EstadoAbierto,EvolucionId,Epicrisis,EmpleadoRegistraId")] Episodio episodio)
        {
            if (id != episodio.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Validar que la fecha y hora de alta sea posterior a la fecha actual
                if (episodio.FechayHoraAlta <= DateTime.Now)
                {
                    ModelState.AddModelError("FechayHoraAlta", "La fecha y hora de alta debe ser posterior a la fecha actual");
                    ViewData["EmpleadoRegistraId"] = new SelectList(_context.Empleados, "Id", "Fullname", episodio.EmpleadoRegistraId);
                    return View(episodio);
                }
                try
                {
                    var episodioEnDB = _context.Episodios.Find(episodio.Id);
                    bool estadoAbiertoPrevio = episodioEnDB.EstadoAbierto;
                    if(episodioEnDB!=null)
                    {
                        episodioEnDB.Motivo = episodio.Motivo;
                        episodioEnDB.Descripcion = episodio.Descripcion;
                        episodioEnDB.EstadoAbierto = episodio.EstadoAbierto;
                        episodioEnDB.FechayHoraAlta = episodio.FechayHoraAlta;

                        if(estadoAbiertoPrevio && !episodioEnDB.EstadoAbierto)
                        {
                            episodioEnDB.FechayHoraCierre = DateTime.Now;
                        }else if(episodioEnDB.EstadoAbierto)
                        {
                            episodioEnDB.FechayHoraCierre = null;
                        }

                        _context.Update(episodioEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound(); 
                    }
                    return RedirectToAction("Index", "Episodios", new { pacienteid = episodioEnDB.PacienteId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodioExists(episodio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["EmpleadoRegistraId"] = new SelectList(_context.Empleados, "Id", "Fullname", episodio.EmpleadoRegistraId);
            return View(episodio);
        }
        #endregion

        #region Delete
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Episodios == null)
            {
                return NotFound();
            }
            var episodio =  _context.Episodios
                .Include(e => e.EmpleadoRegistra)
                .Include(e => e.Epicrisis)
                .FirstOrDefault(m => m.Id == id);
            if (episodio == null)
            {
                return NotFound();
            }

            return View(episodio);
        }

        [Authorize(Roles = $"{Config.AdminRolName}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Episodios == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var episodio =  _context.Episodios.Find(id);
            if (episodio != null)
            {
                _context.Episodios.Remove(episodio);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool EpisodioExists(int id)
        {
          return _context.Episodios.Any(e => e.Id == id);
        }
        private bool existeEvolucion(int id)
        {
            var existeEvolucion = _context.Evoluciones.Any(e => e.EpisodioId == id);
            return existeEvolucion;
        }
        #endregion

        #region Cerrar Episodio
        [HttpGet]
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        public IActionResult Cerrar(int? id)
        {
            if (id == null || _context.Episodios == null)
            {
                return NotFound();
            }
            var episodio = _context.Episodios.Find(id);
            if (episodio == null)
            {
                return NotFound();
            }
            if (episodio.EstadoAbierto == false) 
            {
                return RedirectToAction("Index", "Episodios", new { pacienteid = episodio.PacienteId });
            }
            ViewData["EmpleadoRegistraId"] = new SelectList(_context.Empleados, "Id", "Fullname", episodio.EmpleadoRegistraId);
            return View(episodio);
        }

        [HttpPost]
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [ValidateAntiForgeryToken]
        public IActionResult Cerrar(int id, [Bind("Id,Motivo,Descripcion,FechaYHoraInicio,FechayHoraAlta,FechayHoraCierre," +
    "EstadoAbierto,EvolucionId,Epicrisis,EmpleadoRegistraId")] Episodio episodio)
        {
            if (id != episodio.Id)
            {
                return NotFound();
            }
            if (episodio.EstadoAbierto)
            {
                try
                {
                    var episodioEnDB = _context.Episodios.Find(episodio.Id);
                    if (episodioEnDB != null)
                    {
                        var evolucionesDelEpisodio = _context.Evoluciones
                            .Where(e => e.EpisodioId == episodioEnDB.Id && e.EstadoAbierto)
                            .ToList();

                        if (evolucionesDelEpisodio.Count == 0)
                        {
                            episodioEnDB.EstadoAbierto = false;
                            episodioEnDB.FechayHoraCierre = DateTime.Now;

                            _context.Update(episodioEnDB);
                            _context.SaveChanges();

                            Epicrisis epicrisis = new Epicrisis()
                            {
                                EpisodioId = id,
                                MedicoId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                                fechaYHora = DateTime.Now,
                            };

                            _context.Epicrises.Add(epicrisis);
                            _context.SaveChanges();

                            return RedirectToAction("Create", "Diagnosticos", new { EpicrisisId = epicrisis.Id });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "No se puede cerrar el episodio. Todas las evoluciones deben estar cerradas.");
                            return View(episodio);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodioExists(episodio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion
    }
}
