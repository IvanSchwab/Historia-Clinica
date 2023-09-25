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
using Historia_Clinica.Helpers;

namespace Historia_Clinica.Controllers
{
    [Authorize]
    public class EvolucionesController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;

        public EvolucionesController(HistoriaClinicaContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}, {Config.MedicoRolName}, {Config.PacienteRolname}")]
        public IActionResult Index(int? episodioId)
        {
            if (episodioId != null)
            {
                var historiaClinicaContext = _context.Evoluciones.Include(e => e.Medico).Where(e => e.EpisodioId.Equals(episodioId)).ToList();
                return View(historiaClinicaContext.ToList());

            }
            else
            {
                var historiaClinicaContext = _context.Evoluciones.Include(e => e.Medico).ToList();
                return View(historiaClinicaContext.ToList());
            }
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Evoluciones == null)
            {
                return NotFound();
            }

            var evolucion = _context.Evoluciones
                .Include(e => e.Medico)
                .FirstOrDefault(m => m.Id == id);
            if (evolucion == null)
            {
                return NotFound();
            }

            return View(evolucion);
        }
        #endregion

        #region Create
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [HttpGet]
        public IActionResult Create(int? episodioId)
        {
            if(episodioId == null)
            {
                return NotFound();
            }

            var episodioEnDB = _context.Episodios.Find(episodioId);
            if (episodioEnDB.EstadoAbierto == false)
            {
                return RedirectToAction("Index", "Episodios", new { pacienteid = episodioEnDB.PacienteId });
            }
            Evolucion evolucion = new Evolucion();
            evolucion.EpisodioId = episodioId.Value;
            evolucion.MedicoId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            return View(evolucion);
        }

        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DescripcionAtencion, MedicoId, EpisodioId, FechayHoraAlta")]
Evolucion evolucion)
        {
            if (ModelState.IsValid)
            {
                DateTime hora = DateTime.Now;
                evolucion.EstadoAbierto = true;

                if (evolucion.FechayHoraAlta < hora)
                {
                    ModelState.AddModelError("FechayHoraAlta", "La fecha debe ser posterior a la fecha actual.");
                    return View(evolucion);
                }

                // Agregar la evolución al contexto y guardar los cambios
                _context.Add(evolucion);
                _context.SaveChanges();

                // Actualizar el estado del episodio a cerrado si todas las evoluciones están cerradas
                var episodio = _context.Episodios.Find(evolucion.EpisodioId);
                if (episodio != null && !_context.Evoluciones.Any(e => e.EpisodioId == episodio.Id && e.EstadoAbierto))
                {
                    episodio.EstadoAbierto = false;
                    _context.Episodios.Update(episodio);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index", "Evoluciones", new { EpisodioId = evolucion.EpisodioId });
            }

            return View(evolucion);
        }

        #endregion

        #region Edit
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Evoluciones == null)
            {
                return NotFound();
            }

            var evolucion = _context.Evoluciones.Find(id);
            if (evolucion == null)
            {
                return NotFound();
            }
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Id", "DNI", evolucion.MedicoId);
            return View(evolucion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        public IActionResult Edit(int id, [Bind("Id,DescripcionAtencion,EstadoAbierto,MedicoId")] 
        Evolucion evolucion)
        {
            if (id != evolucion.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var evolucionEnDB = _context.Evoluciones.Find(evolucion.Id);
                    if (evolucionEnDB != null)
                    {
                        evolucionEnDB.DescripcionAtencion = evolucion.DescripcionAtencion;
                        evolucionEnDB.EstadoAbierto = evolucion.EstadoAbierto;
                        _context.Evoluciones.Update(evolucionEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvolucionExists(evolucion.Id))
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
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Id", "DNI", evolucion.MedicoId);
            return View(evolucion);
        }
        #endregion

        #region Detele
        [HttpGet]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Evoluciones == null)
            {
                return NotFound();
            }
            var evolucion = _context.Evoluciones
                .Include(e => e.Medico)
                .FirstOrDefault(m => m.Id == id);
            if (evolucion == null)
            {
                return NotFound();
            }
            return View(evolucion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.EmpleadoRolName}, {Config.AdminRolName}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Evoluciones == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var evolucion = _context.Evoluciones.Find(id);
            if (evolucion != null)
            {
                _context.Evoluciones.Remove(evolucion);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool EvolucionExists(int id)
        {
          return _context.Evoluciones.Any(e => e.Id == id);
        }
        #endregion

        #region Cerrar Evolucion
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [HttpGet]
        public IActionResult Cerrar(int? id)
        {
            if (id == null || _context.Evoluciones == null)
            {
                return NotFound();
            }

            var evolucion = _context.Evoluciones.Find(id);
            if (evolucion == null)
            {
                return NotFound();
            }

            return View(evolucion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        public IActionResult Cerrar(int id, [Bind("Id, DescripcionAtencion, EstadoAbierto, FechayHoraCierre, MedicoId")]
    Evolucion evolucion)
        {
            if (id != evolucion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var evolucionEnDB = _context.Evoluciones.Find(evolucion.Id);
                    if (evolucionEnDB != null)
                    {
                        evolucionEnDB.DescripcionAtencion = evolucion.DescripcionAtencion;
                        evolucionEnDB.EstadoAbierto = false;
                        evolucionEnDB.FechayHoraCierre = DateTime.Now;

                        _context.Evoluciones.Update(evolucionEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvolucionExists(evolucion.Id))
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

            ViewData["MedicoId"] = new SelectList(_context.Medico, "Id", "DNI", evolucion.MedicoId);
            return View(evolucion);
        }

        #endregion
    }
}
