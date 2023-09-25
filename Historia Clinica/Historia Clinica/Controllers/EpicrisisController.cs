using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Historia_Clinica.Data;
using Historia_Clinica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Historia_Clinica.Helpers;

namespace Historia_Clinica.Controllers
{
    [Authorize]
    public class EpicrisisController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;
        public EpicrisisController(HistoriaClinicaContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        [Authorize(Roles = $"{Config.PacienteRolname}, {Config.AdminRolName}, {Config.MedicoRolName}")]
        [HttpGet]
        public IActionResult Index(int? episodioId)
        {
            if (episodioId != null)
            {
                var historiaClinicaContext = _context.Epicrises.Include(e => e.Medico).Where(e => e.EpisodioId.Equals(episodioId)).ToList();
                return View(historiaClinicaContext);
            }
            else
            {
                var historiaClinicaContext = _context.Epicrises.Include(e => e.Medico).ToList();
                return View(historiaClinicaContext);
            }
        }
        #endregion

        #region Details
        [Authorize(Roles = $"{Config.PacienteRolname}, {Config.MedicoRolName}, {Config.AdminRolName}")]
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Epicrises == null)
            {
                return NotFound();
            }

            var epicrisis = _context.Epicrises
                .Include(e => e.Medico)
                .FirstOrDefault(m => m.Id == id);
            if (epicrisis == null)
            {
                return NotFound();
            }

            return View(epicrisis);
        }
        #endregion

        #region Create
        [Authorize(Roles = $"{Config.AdminRolName}")]
        [HttpGet]
        public IActionResult Create(int? episodioId)
        {

            if (episodioId == null)
            {
                return NotFound();
            }
            Epicrisis epicrisis = new Epicrisis();
            epicrisis.EpisodioId = episodioId.Value;
            epicrisis.MedicoId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            return View(epicrisis);
        }

        [HttpPost]
        [Authorize(Roles = $"{Config.MedicoRolName}, {Config.AdminRolName}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,EpisodioId,MedicoId,fechaYHora,Diagnostico")] Epicrisis epicrisis)
        {
            // Verificar si todas las evoluciones del episodio están cerradas
            bool evolucionesCerradas = _context.Evoluciones
                .Where(e => e.EpisodioId == epicrisis.EpisodioId)
                .All(e => e.EstadoAbierto);

            if (!evolucionesCerradas)
            {
                ModelState.AddModelError(string.Empty, "No se puede crear la epicrisis. Todas las evoluciones deben estar cerradas.");
                return View(epicrisis);
            }

            // Verificar si el episodio está abierto
            Episodio episodio = _context.Episodios.Find(epicrisis.EpisodioId);
            if (episodio == null || episodio.EstadoAbierto)
            {
                ModelState.AddModelError(string.Empty, "No se puede crear la epicrisis. El episodio no está abierto.");
                return View(epicrisis);
            }

            if (ModelState.IsValid)
            {
                // Asignar la fecha y hora actual
                epicrisis.fechaYHora = DateTime.Now;

                // Cerrar automáticamente el episodio
                episodio.EstadoAbierto = true;
                _context.Update(episodio);

                _context.Add(epicrisis);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(epicrisis);
        }
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Epicrises == null)
            {
                return NotFound();
            }
            var epicrisis = _context.Epicrises.Find(id);
            if (epicrisis == null)
            {
                return NotFound();
            }
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Id", "Fullname", epicrisis.MedicoId);
            return View(epicrisis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Edit(int id, [Bind("Id,MedicoId,fechaYHora")] Epicrisis epicrisis)
        {
            if (id != epicrisis.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var epicrisisEnDB = _context.Epicrises.Find(id);
                    if (epicrisisEnDB != null)
                    {
                        _context.Update(epicrisisEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpicrisisExists(epicrisis.Id))
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
            ViewData["MedicoId"] = new SelectList(_context.Medico, "Id", "Fullname", epicrisis.MedicoId);
            return View(epicrisis);
        }
        #endregion

        #region Delete
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Epicrises == null)
            {
                return NotFound();
            }
            var epicrisis = _context.Epicrises
                .Include(e => e.Medico)
                .FirstOrDefault(m => m.Id == id);
            if (epicrisis == null)
            {
                return NotFound();
            }
            return View(epicrisis);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Epicrises == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var epicrisis = _context.Epicrises.Find(id);
            if (epicrisis != null)
            {
                _context.Epicrises.Remove(epicrisis);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool EpicrisisExists(int id)
        {
          return _context.Epicrises.Any(e => e.Id == id);
        }
        #endregion
    }
}
