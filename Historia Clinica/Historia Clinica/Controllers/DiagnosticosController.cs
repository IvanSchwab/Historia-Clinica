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
    public class DiagnosticosController : Controller
    {
        #region Properties & Constructor
        private readonly HistoriaClinicaContext _context;
        public DiagnosticosController(HistoriaClinicaContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.EmpleadoRolName},  {Config.MedicoRolName}, {Config.PacienteRolname}")]
        public IActionResult Index(int? EpicrisisId)
        {
            if (EpicrisisId != null)
            {
                var historiaClinicaContext = _context.Diagnosticos.Include(d => d.Epicrisis).Where(e => e.EpicrisisId.Equals(EpicrisisId)).ToList();
                return View(historiaClinicaContext);
            }
            else
            {
               var historiaClinicaContext = _context.Diagnosticos.Include(d => d.Epicrisis).ToList();
                return View(historiaClinicaContext);
            }
        }
        #endregion

        #region Details
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.EmpleadoRolName},  {Config.MedicoRolName}")]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Diagnosticos == null)
            {
                return NotFound();
            }

            var diagnostico =  _context.Diagnosticos
                .Include(d => d.Epicrisis)
                .FirstOrDefault(m => m.Id == id);
            if (diagnostico == null)
            {
                return NotFound();
            }

            return View(diagnostico);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        public IActionResult Create(int? epicrisisId)
        {
            if (epicrisisId == null)
            {
                return NotFound();
            }
            else
            {
                Diagnostico diagnostico = new Diagnostico();
                diagnostico.EpicrisisId = epicrisisId.Value;
                return View(diagnostico);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        #region ValidateAntiForgeryToken
        //Especifica que la clase o el método que se aplica este atributo valida el token antifalsificación.
        //Si el token antifalsificación no está disponible o si el token no es válido, se producirá un error
        //en la validación y el método de acción no se ejecutará.
        #endregion
        public IActionResult Create([Bind("Id,EpicrisisId,Descripcion,Recomendacion")] Diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                var epicrisis = _context.Epicrises.Find(diagnostico.EpicrisisId);
                _context.Diagnosticos.Add(diagnostico);
                _context.SaveChanges();
                return RedirectToAction("Index", "Epicrisis", new { EpisodioId = epicrisis.EpisodioId});
            }
            else 
            { 
            return View(diagnostico);
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Diagnosticos == null)
            {
                return NotFound();
            }
            var diagnostico = _context.Diagnosticos.Find(id);
            if (diagnostico == null)
            {
                return NotFound();
            }
            ViewData["EpicrisisId"] = new SelectList(_context.Epicrises, "Id", "Id", diagnostico.EpicrisisId);
            return View(diagnostico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        public IActionResult Edit(int id, [Bind("Id,EpicrisisId,Descripcion,Recomendacion")] Diagnostico diagnostico)
        {
            if (id != diagnostico.Id)
            {
                return NotFound();
            }
            else 
            { 
            if (ModelState.IsValid)
            {
                try
                {   
                    var diagnosticoEnDB = _context.Diagnosticos.Find(id);
                    if(diagnostico != null)
                    {
                        diagnosticoEnDB.Descripcion = diagnostico.Descripcion;
                        diagnosticoEnDB.Recomendacion = diagnostico.Recomendacion;
                        _context.Update(diagnosticoEnDB);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosticoExists(diagnostico.Id))
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
            ViewData["EpicrisisId"] = new SelectList(_context.Epicrises, "Id", "Id", diagnostico.EpicrisisId);
            return View(diagnostico);
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Diagnosticos == null)
            {
                return NotFound();
            }
            var diagnostico =  _context.Diagnosticos
                .Include(d => d.Epicrisis)
                .FirstOrDefault(m => m.Id == id);
            if (diagnostico == null)
            {
                return NotFound();
            }
            return View(diagnostico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Config.AdminRolName},{Config.MedicoRolName}")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Diagnosticos == null)
            {
                return Problem(ErrorMsg.HistoriaClinicaIsNull);
            }
            var diagnostico =  _context.Diagnosticos.Find(id);
            if (diagnostico != null)
            {
                _context.Diagnosticos.Remove(diagnostico);
            }
            
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Methods
        private bool DiagnosticoExists(int id)
        {
          return _context.Diagnosticos.Any(e => e.Id == id);
        }
        #endregion
    }
}
